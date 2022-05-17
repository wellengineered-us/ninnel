/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;

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
				Type ninnelContextType = pipelineConfiguration.Parent.GetContextType();

				using (INinnelContext ninnelContext = ninnelContextFactory.CreateContext(ninnelContextType))
				{
					ninnelContext.Configuration = pipelineConfiguration;
					ninnelContext.Create();

					ninnelPipeline.Execute(ninnelContext);
				}

				return ninnelPipeline; // return the reference but it will be disposed...
			}
		}

		protected override void CoreCancel()
		{
			// this method gets called on another thread from Main()...
			Console.Out.WriteLine("halt_and_catch_fire: enter");
			this.CoreGracefulShutdown(false);
			Console.Out.WriteLine("halt_and_catch_fire: leave");
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

		protected override INinnelPipeline CoreCreatePipeline(Type ninnelPipelineType)
		{
			// TODO - Fix this
			return new DefaultComponentFactory().CreateNinnelComponent<INinnelPipeline>(AssemblyDomain.Default.DependencyManager, ninnelPipelineType, (this.Configuration.ComponentAutoWire ?? true));
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

		protected virtual void CoreExecutePipelineOnce(SemaphoreSlim semaphoreSlim, Type ninnelPipelineType, PipelineConfiguration pipelineConfiguration)
		{
			const bool USE_THREAD_POOL = false;

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
				//Thread.Sleep(10000);

				if ((object)semaphoreSlim != null)
					semaphoreSlim.Release();
			}
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
				Guid? __ = Guid.Empty;
				using (IDisposableDispatch<SemaphoreSlim> semaphoreSlim = AssemblyDomain.Default.ResourceManager.Using(__, new SemaphoreSlim(0, pipelineCount)))
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

						this.CoreExecutePipelineOnce(semaphoreSlim.Target, ninnelPipelineType, pipelineConfiguration);
					}

					//semaphoreSlim.Wait(this.CancellationTokenSource.Token);

					//using (CancellationTokenSource ctsDie = new CancellationTokenSource())
					{
						//TimeSpan gracefulShutdownTimeSpan;

						//gracefulShutdownTimeSpan = this.Configuration.GracefulShutdownTimeSpan ??
						//new TimeSpan(0, 0, 0, 5, 0);

						//ctsDie.CancelAfter(gracefulShutdownTimeSpan);
						//semaphoreSlim.Wait(ctsDie.Token);
						semaphoreSlim.Target.Wait();
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

		protected virtual void CoreMaybeDispatchAfter()
		{
			Console.Out.WriteLine("dispatch_loop: end");
		}

		protected virtual void CoreMaybeDispatchAwait()
		{
			Console.Out.WriteLine("dispatch_loop: await");
		}

		protected virtual void CoreMaybeDispatchBefore()
		{
			Console.Out.WriteLine("dispatch_loop: begin");
		}

		protected virtual void CoreMaybeDispatchIdle()
		{
			TimeSpan dispatchIdleTimeSpan;

			dispatchIdleTimeSpan = this.Configuration.DispatchIdleTimeSpan ?? new TimeSpan(0, 0, 0, 10, 0);

			Console.Out.WriteLine(string.Format("dispatch_loop: idle begin: timespan(ms) = {0}, timespan ='{1}'", dispatchIdleTimeSpan.TotalMilliseconds, dispatchIdleTimeSpan));
			Thread.Sleep(dispatchIdleTimeSpan);
			Console.Out.WriteLine("dispatch_loop: idle end");
		}

		protected bool ShouldRunDispatchLoop()
		{
			bool enabled, cancellation;
			enabled = (this.Configuration.EnableDispatchLoop ?? false);
			cancellation = this.CancellationTokenSource.IsCancellationRequested;

			Console.Out.WriteLine(string.Format("should_run_dispatch_loop: enabled = {0}, cancellation = {1}", enabled, cancellation));

			return enabled && !cancellation;
		}

		#endregion
	}
}