/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Context;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Transport;
using WellEngineered.Solder.Injection;

namespace WellEngineered.Ninnel.Hosting.Tool.Defaults
{
	public partial class DefaultToolHost
		: NinnelToolHost
	{
		#region Methods/Operators

		protected static async ValueTask<INinnelPipeline> CoreCreateExecuteThenDisposePipelineAsync(INinnelPipelineFactory ninnelPipelineFactory, Type ninnelPipelineType, PipelineConfiguration pipelineConfiguration, CancellationToken cancellationToken = default)
		{
			if (ninnelPipelineFactory == null)
				throw new ArgumentNullException(nameof(ninnelPipelineFactory));

			if (ninnelPipelineType == null)
				throw new ArgumentNullException(nameof(ninnelPipelineType));

			if (pipelineConfiguration == null)
				throw new ArgumentNullException(nameof(pipelineConfiguration));

			await using (INinnelPipeline ninnelPipeline = await ninnelPipelineFactory.CreatePipelineAsync(ninnelPipelineType, cancellationToken))
			{
				ninnelPipeline.Configuration = pipelineConfiguration;
				await ninnelPipeline.CreateAsync();

				INinnelContextFactory ninnelContextFactory = ninnelPipeline;
				Type ninnelContextType = pipelineConfiguration.Parent.GetContextType();

				using (INinnelContext ninnelContext = await ninnelContextFactory.CreateContextAsync(ninnelContextType, cancellationToken))
				{
					ninnelContext.Configuration = pipelineConfiguration;
					await ninnelContext.CreateAsync();

					await ninnelPipeline.ExecuteAsync(ninnelContext, cancellationToken);
				}

				return ninnelPipeline; // return the reference but it will be disposed...
			}
		}

		protected override async ValueTask CoreCancelAsync(CancellationToken cancellationToken = default)
		{
			// this method gets called on another thread from Main()...
			await Console.Out.WriteLineAsync("halt_and_catch_fire(async): enter");
			await this.CoreGracefulShutdownAsync(false);
			await Console.Out.WriteLineAsync("halt_and_catch_fire(async): leave");
		}

		protected override async ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			if (this.IsAsyncCreated)
				return;

			if (creating)
			{
				await base.CoreCreateAsync(creating, cancellationToken); // intentionally placed here

				this.CancellationTokenSource.Token.ThrowIfCancellationRequested();
			}
		}

		protected override async ValueTask<INinnelPipeline> CoreCreatePipelineAsync(Type ninnelPipelineType, CancellationToken cancellationToken = default)
		{
			// TODO - Fix this
			return await new DefaultComponentFactory().CreateNinnelComponentAsync<INinnelPipeline>(AssemblyDomain.Default.DependencyManager, ninnelPipelineType, (this.Configuration.ComponentAutoWire ?? true), null, true, cancellationToken);
		}

		protected override async ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			if (this.IsAsyncDisposed)
				return;

			if (disposing)
			{
				await this.CoreGracefulShutdownAsync(true);

				this.CancellationTokenSource.Dispose();

				await base.CoreDisposeAsync(disposing, cancellationToken);
			}
		}

		protected virtual async ValueTask CoreExecutePipelineOnceAndReleaseAsync(Type ninnelPipelineType, PipelineConfiguration pipelineConfiguration, CancellationToken cancellationToken = default)
		{
			if (ninnelPipelineType == null)
				throw new ArgumentNullException(nameof(ninnelPipelineType));

			if (pipelineConfiguration == null)
				throw new ArgumentNullException(nameof(pipelineConfiguration));

			try
			{
				await CoreCreateExecuteThenDisposePipelineAsync(this, ninnelPipelineType, pipelineConfiguration, cancellationToken);
			}
			finally
			{
				await Task.Delay(1000, cancellationToken);
			}
		}

		protected virtual async ValueTask CoreExecutePipelineOnceAsync(Type ninnelPipelineType, PipelineConfiguration pipelineConfiguration, CancellationToken cancellationToken = default)
		{
			if (ninnelPipelineType == null)
				throw new ArgumentNullException(nameof(ninnelPipelineType));

			if (pipelineConfiguration == null)
				throw new ArgumentNullException(nameof(pipelineConfiguration));

			await this.CoreExecutePipelineOnceAndReleaseAsync(ninnelPipelineType, pipelineConfiguration, cancellationToken);
		}

		protected virtual async ValueTask CoreGracefulShutdownAsync(bool disposing)
		{
			TimeSpan gracefulShutdownTimeSpan;

			gracefulShutdownTimeSpan = this.Configuration.GracefulShutdownTimeSpan ??
										new TimeSpan(0, 0, 0, 5, 0);

			if (this.CancellationTokenSource.Token.IsCancellationRequested)
				return;

			if (!this.CancellationTokenSource.Token.CanBeCanceled)
				throw new NinnelException("!this.CancellationTokenSource.Token.CanBeCanceled");

			await AssemblyDomain.Default.ResourceManager.PrintAsync(Guid.Empty, string.Format("graceful_shutdown(async): disposing = {2}, {0} ms, timespan = '{1}'", gracefulShutdownTimeSpan.TotalMilliseconds, gracefulShutdownTimeSpan, disposing));

			this.CancellationTokenSource.Cancel();
		}

		protected override async ValueTask CoreHostAsync(CancellationToken cancellationToken = default)
		{
			long loopIndex = -1L;

			await this.ShouldRunDispatchLoopAsync(cancellationToken); // nop

			do
			{
				if (++loopIndex > 0L)
				{
					// only called when dispatch loop enabled, and we expect another iteration
					await this.CoreMaybeDispatchIdleAsync(cancellationToken);
				}

				int pipelineCount = this.Configuration.PipelineConfigurations.Count(pc => pc.IsEnabled ?? false);
				Guid? __ = Guid.Empty;
				
				await this.CoreMaybeDispatchBeforeAsync(cancellationToken);

				if (pipelineCount > 0)
				{
					//await using (IDisposableDispatch<SemaphoreSlim> semaphoreSlim = AssemblyDomain.Default.ResourceManager.UsingAsync(__, new SemaphoreSlim(0, pipelineCount)))
					{
						foreach (PipelineConfiguration pipelineConfiguration in this.Configuration.PipelineConfigurations)
						{
							Type ninnelPipelineType;

							if ((object)pipelineConfiguration == null)
								continue;

							if (!(pipelineConfiguration.IsEnabled ?? false))
								continue;

							ninnelPipelineType = pipelineConfiguration.GetPipelineType();

							if ((object)ninnelPipelineType == null)
								throw new NinnelException(nameof(ninnelPipelineType));

							await Console.Out.WriteLineAsync("dispatch_loop(async): execute...");

							await this.CoreExecutePipelineOnceAsync(ninnelPipelineType, pipelineConfiguration, cancellationToken);
						}

						//semaphoreSlim.Wait(this.CancellationTokenSource.Token);

						//using (CancellationTokenSource ctsDie = new CancellationTokenSource())
						{
							//TimeSpan gracefulShutdownTimeSpan;

							//gracefulShutdownTimeSpan = this.Configuration.GracefulShutdownTimeSpan ??
							//new TimeSpan(0, 0, 0, 5, 0);

							//ctsDie.CancelAfter(gracefulShutdownTimeSpan);
							//semaphoreSlim.Wait(ctsDie.Token);
							//semaphoreSlim.Target.Wait();
						}
					}
				}

				await this.CoreMaybeDispatchAfterAsync(cancellationToken);

				await this.CoreMaybeDispatchAwaitAsync(cancellationToken);
			}
			while (await this.ShouldRunDispatchLoopAsync(cancellationToken));
		}

		protected override async ValueTask CoreHostAsync(IDictionary<string, IList<object>> arguments, IDictionary<string, IList<object>> properties, CancellationToken cancellationToken = default)
		{
			await this.CoreHostAsync(cancellationToken);
		}

		protected virtual async ValueTask CoreMaybeDispatchAfterAsync(CancellationToken cancellationToken = default)
		{
			await Console.Out.WriteLineAsync("dispatch_loop(async): end");
		}

		protected virtual async ValueTask CoreMaybeDispatchAwaitAsync(CancellationToken cancellationToken = default)
		{
			await Console.Out.WriteLineAsync("dispatch_loop(async): await");
		}

		protected virtual async ValueTask CoreMaybeDispatchBeforeAsync(CancellationToken cancellationToken = default)
		{
			await Console.Out.WriteLineAsync("dispatch_loop(async): begin");
		}

		protected virtual async ValueTask CoreMaybeDispatchIdleAsync(CancellationToken cancellationToken = default)
		{
			TimeSpan dispatchIdleTimeSpan;

			dispatchIdleTimeSpan = this.Configuration.DispatchIdleTimeSpan ?? new TimeSpan(0, 0, 0, 10, 0);

			await Console.Out.WriteLineAsync(string.Format("dispatch_loop(async): idle begin: timespan(ms) = {0}, timespan ='{1}'", dispatchIdleTimeSpan.TotalMilliseconds, dispatchIdleTimeSpan));
			await Task.Delay(dispatchIdleTimeSpan, cancellationToken);
			await Console.Out.WriteLineAsync("dispatch_loop(async): idle end");
		}

		protected async ValueTask<bool> ShouldRunDispatchLoopAsync(CancellationToken cancellationToken = default)
		{
			bool enabled, cancellation;
			enabled = (this.Configuration.EnableDispatchLoop ?? false);
			cancellation = this.CancellationTokenSource.IsCancellationRequested;

			await Console.Out.WriteLineAsync(string.Format("should_run_dispatch_loop(async): enabled = {0}, cancellation = {1}", enabled, cancellation));

			return enabled && !cancellation;
		}

		#endregion
	}
}
#endif