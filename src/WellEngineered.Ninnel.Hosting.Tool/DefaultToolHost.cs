/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Context;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Transport;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public class DefaultToolHost
		: NinnelToolHost
	{
		#region Constructors/Destructors

		public DefaultToolHost()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		#endregion

		#region Properties/Indexers/Events

		protected CancellationTokenSource CancellationTokenSource
		{
			get
			{
				return this.cancellationTokenSource;
			}
		}

		#endregion

		#region Methods/Operators

		protected static INinnelPipeline CoreCreateExecuteThenDisposePipeline(INinnelPipelineFactory ninnelPipelineFactory, Type ninnelPipelineType, PipelineConfiguration pipelineConfiguration)
		{
			if (ninnelPipelineFactory == null)
				throw new ArgumentNullException(nameof(ninnelPipelineFactory));

			if (ninnelPipelineType == null)
				throw new ArgumentNullException(nameof(ninnelPipelineType));

			if (pipelineConfiguration == null)
				throw new ArgumentNullException(nameof(pipelineConfiguration));

			using (INinnelPipeline ninnelPipeline = ninnelPipelineFactory.CreatePipeline(ninnelPipelineType))
			{
				ninnelPipeline.Configuration = pipelineConfiguration;
				ninnelPipeline.Create();

				INinnelContextFactory ninnelContextFactory = ninnelPipeline;

				using (INinnelContext ninnelContext = ninnelContextFactory.CreateContext())
				{
					ninnelContext.Configuration = pipelineConfiguration;
					ninnelContext.Create();

					ninnelPipeline.Execute(ninnelContext);
				}

				return ninnelPipeline; // return the reference but it will be disposed...
			}
		}

		protected static ValueTask<INinnelPipeline> CoreCreateExecuteThenDisposePipelineAsync(INinnelPipelineFactory ninnelPipelineFactory, Type ninnelPipelineType, PipelineConfiguration pipelineConfiguration)
		{
			return default;
		}

		protected override void CoreCancel()
		{
			// this method gets called on another thread from Main()...
			Console.Out.WriteLine("halt_and_catch_fire: enter");
			this.CoreGracefulShutdown(false);
			Console.Out.WriteLine("halt_and_catch_fire: leave");
		}

		protected override ValueTask CoreCancelAsync(CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override void CoreCreate(bool creating)
		{
			if (this.IsCreated)
				return;

			if (creating)
			{
				base.CoreCreate(creating); // intentionally placed here

				this.CancellationTokenSource.Token.ThrowIfCancellationRequested();
			}
		}

		protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			return base.CoreCreateAsync(creating, cancellationToken);
		}

		protected override INinnelPipeline CoreCreatePipeline(Type ninnelPipelineType)
		{
			INinnelPipeline ninnelPipeline;
			bool autoWire;

			if ((object)ninnelPipelineType == null)
				throw new ArgumentNullException(nameof(ninnelPipelineType));

			if (autoWire = (this.Configuration.HostAutoWire ?? true))
				ninnelPipeline = ninnelPipelineType.ResolveAutoWireAssignableToTargetType<INinnelPipeline>(AssemblyDomain.Default.DependencyManager);
			else
				ninnelPipeline = ninnelPipelineType.CreateInstanceAssignableToTargetType<INinnelPipeline>();

			if ((object)ninnelPipeline == null)
				throw new NinnelException(string.Format("Failed to instantiate pipeline type: '{0}', auto-wire: {1}.", ninnelPipelineType.FullName, autoWire));

			return ninnelPipeline;
		}

		protected override ValueTask<INinnelPipeline> CoreCreatePipelineAsync(Type ninnelPipelineType, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override void CoreDispose(bool disposing)
		{
			if (this.IsDisposed)
				return;

			if (disposing)
			{
				this.CoreGracefulShutdown(true);

				this.CancellationTokenSource.Dispose();
			}

			base.CoreDispose(disposing);
		}

		protected override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			return base.CoreDisposeAsync(disposing, cancellationToken);
		}

		protected virtual void CoreExecutePipelineOnce(SemaphoreSlim semaphoreSlim, Type ninnelPipelineType, PipelineConfiguration pipelineConfiguration)
		{
			const bool USE_THREAD_POOL = true;

			if ((object)semaphoreSlim == null)
				throw new ArgumentNullException(nameof(semaphoreSlim));

			if (ninnelPipelineType == null)
				throw new ArgumentNullException(nameof(ninnelPipelineType));

			if (pipelineConfiguration == null)
				throw new ArgumentNullException(nameof(pipelineConfiguration));

			if (USE_THREAD_POOL)
			{
				if (!ThreadPool.QueueUserWorkItem((o) => this.CoreExecutePipelineOnceAndRelease(semaphoreSlim, ninnelPipelineType, pipelineConfiguration)))
					throw new NinnelException(string.Format("{0}::{1} failed.", nameof(ThreadPool), nameof(ThreadPool.QueueUserWorkItem)));
			}
			else
				this.CoreExecutePipelineOnceAndRelease(semaphoreSlim, ninnelPipelineType, pipelineConfiguration);
		}

		protected virtual void CoreExecutePipelineOnceAndRelease(SemaphoreSlim semaphoreSlim, Type ninnelPipelineType, PipelineConfiguration pipelineConfiguration)
		{
			if ((object)semaphoreSlim == null)
				throw new ArgumentNullException(nameof(semaphoreSlim));

			if (ninnelPipelineType == null)
				throw new ArgumentNullException(nameof(ninnelPipelineType));

			if (pipelineConfiguration == null)
				throw new ArgumentNullException(nameof(pipelineConfiguration));

			try
			{
				CoreCreateExecuteThenDisposePipeline(this, ninnelPipelineType, pipelineConfiguration);
			}
			finally
			{
				Thread.Sleep(10000);

				if ((object)semaphoreSlim != null)
					semaphoreSlim.Release();
			}
		}

		protected virtual ValueTask CoreExecutePipelineOnceAsync(Type ninnelPipelineType, PipelineConfiguration pipelineConfiguration)
		{
			return default;
		}

		protected virtual void CoreGracefulShutdown(bool disposing)
		{
			TimeSpan gracefulShutdownTimeSpan;

			gracefulShutdownTimeSpan = this.Configuration.GracefulShutdownTimeSpan ??
										new TimeSpan(0, 0, 0, 5, 0);

			if (this.CancellationTokenSource.Token.IsCancellationRequested)
				return;

			if (!this.CancellationTokenSource.Token.CanBeCanceled)
				throw new NinnelException("!this.CancellationTokenSource.Token.CanBeCanceled");

			AssemblyDomain.Default.ResourceManager.Print(Guid.Empty, string.Format("graceful_shutdown: disposing = {2}, {0} ms, timespan = '{1}'", gracefulShutdownTimeSpan.TotalMilliseconds, gracefulShutdownTimeSpan, disposing));

			this.CancellationTokenSource.Cancel();
		}

		protected virtual ValueTask CoreGracefulShutdownAsync(bool disposing)
		{
			return default;
		}

		protected override void CoreHost()
		{
			long loopIndex = -1L;

			this.ShouldRunDispatchLoop(); // nop

			do
			{
				if (++loopIndex > 0L)
				{
					// only called when dispatch loop enabled, and we expect another iteration
					this.CoreMaybeDispatchIdle();
				}

				this.CoreMaybeDispatchBefore();

				int pipelineCount = this.Configuration.PipelineConfigurations.Count;
				using (SemaphoreSlim semaphoreSlim = new SemaphoreSlim(0, pipelineCount))
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

						Console.Out.WriteLine("dispatch_loop: execute...");

						this.CoreExecutePipelineOnce(semaphoreSlim, ninnelPipelineType, pipelineConfiguration);
					}

					//semaphoreSlim.Wait(this.CancellationTokenSource.Token);

					//using (CancellationTokenSource ctsDie = new CancellationTokenSource())
					{
						//TimeSpan gracefulShutdownTimeSpan;

						//gracefulShutdownTimeSpan = this.Configuration.GracefulShutdownTimeSpan ??
						//new TimeSpan(0, 0, 0, 5, 0);

						//ctsDie.CancelAfter(gracefulShutdownTimeSpan);
						//semaphoreSlim.Wait(ctsDie.Token);
						semaphoreSlim.Wait();
					}
				}

				this.CoreMaybeDispatchAfter();

				this.CoreMaybeDispatchAwait();
			}
			while (this.ShouldRunDispatchLoop());
		}

		protected override void CoreHost(IDictionary<string, IList<object>> arguments, IDictionary<string, IList<object>> properties)
		{
			this.CoreHost();
		}

		protected override ValueTask CoreHostAsync(CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override ValueTask CoreHostAsync(IDictionary<string, IList<object>> arguments, IDictionary<string, IList<object>> properties, CancellationToken cancellationToken = default)
		{
			return this.CoreHostAsync();
		}

		protected virtual void CoreMaybeDispatchAfter()
		{
			Console.Out.WriteLine("dispatch_loop: end");
		}

		protected virtual async ValueTask CoreMaybeDispatchAfterAsync()
		{
			await Console.Out.WriteLineAsync("dispatch_loop: end");
		}

		protected virtual void CoreMaybeDispatchAwait()
		{
			Console.Out.WriteLine("dispatch_loop: await");
		}

		protected virtual async ValueTask CoreMaybeDispatchAwaitAsync()
		{
			await Console.Out.WriteLineAsync("dispatch_loop: await");
		}

		protected virtual void CoreMaybeDispatchBefore()
		{
			Console.Out.WriteLine("dispatch_loop: begin");
		}

		protected virtual async ValueTask CoreMaybeDispatchBeforeAsync()
		{
			await Console.Out.WriteLineAsync("dispatch_loop: begin");
		}

		protected virtual void CoreMaybeDispatchIdle()
		{
			TimeSpan dispatchIdleTimeSpan;

			dispatchIdleTimeSpan = this.Configuration.DispatchIdleTimeSpan ?? new TimeSpan(0, 0, 0, 10, 0);

			Console.Out.WriteLine(string.Format("dispatch_loop: idle begin: timespan(ms) = {0}, timespan ='{1}'", dispatchIdleTimeSpan.TotalMilliseconds, dispatchIdleTimeSpan));
			Thread.Sleep(dispatchIdleTimeSpan);
			Console.Out.WriteLine("dispatch_loop: idle end");
		}

		protected virtual async ValueTask CoreMaybeDispatchIdleAsync()
		{
			TimeSpan dispatchIdleTimeSpan;

			dispatchIdleTimeSpan = this.Configuration.DispatchIdleTimeSpan ?? new TimeSpan(0, 0, 0, 10, 0);

			await Console.Out.WriteLineAsync(string.Format("dispatch_loop: idle begin: timespan(ms) = {0}, timespan ='{1}'", dispatchIdleTimeSpan.TotalMilliseconds, dispatchIdleTimeSpan));
			await Task.Delay(dispatchIdleTimeSpan);
			await Console.Out.WriteLineAsync("dispatch_loop: idle end");
		}

		protected bool ShouldRunDispatchLoop()
		{
			bool enabled, cancellation;
			enabled = (this.Configuration.EnableDispatchLoop ?? false);
			cancellation = this.CancellationTokenSource.IsCancellationRequested;

			Console.Out.WriteLine(string.Format("should_run_dispatch_loop: enabled = {0}, cancellation = {1}", enabled, cancellation));

			return enabled && !cancellation;
		}

		protected async ValueTask<bool> ShouldRunDispatchLoopAsync()
		{
			bool enabled, cancellation;
			enabled = (this.Configuration.EnableDispatchLoop ?? false);
			cancellation = this.CancellationTokenSource.IsCancellationRequested;

			await Console.Out.WriteLineAsync(string.Format("should_run_dispatch_loop: enabled = {0}, cancellation = {1}", enabled, cancellation));

			return enabled && !cancellation;
		}

		#endregion
	}
}