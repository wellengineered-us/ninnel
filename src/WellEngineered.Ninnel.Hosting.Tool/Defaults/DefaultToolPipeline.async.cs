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
using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Middleware;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Ninnel.Station;
using WellEngineered.Ninnel.Transport;
using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Injection;

namespace WellEngineered.Ninnel.Hosting.Tool.Defaults
{
	public partial class DefaultToolPipeline
		: NinnelPipeline
	{
		#region Methods/Operators

		protected override async ValueTask<INinnelContext> CoreCloneContextAsync(INinnelContext ninnelContext, CancellationToken cancellationToken = default)
		{
			await Task.CompletedTask;

			// for now, just return this one
			return ninnelContext;
		}

		protected override async ValueTask<INinnelContext> CoreCreateContextAsync(Type ninnelContextType, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelContextType == null)
				throw new ArgumentNullException(nameof(ninnelContextType));

			// TODO - Fix this
			return await new DefaultComponentFactory().CreateNinnelComponentAsync<INinnelContext>(AssemblyDomain.Default.DependencyManager, ninnelContextType, (this.Configuration.Parent.ComponentAutoWire ?? true), null, true, cancellationToken);
		}

		protected override async ValueTask<long> CoreExecuteAsync(INinnelContext ninnelContext, CancellationToken cancellationToken = default)
		{
			IAsyncNinnelStream asyncNinnelStream;

			INinnelInletStation ninnelInletStation;
			INinnelOutletStation ninnelOutletStation;

			Type ninnelInletStationType;
			Type ninnelOutletStationType;
			IDictionary<IUnknownNinnelConfiguration, Type> middlewareConfigTypeMappings;

			if ((object)ninnelContext == null)
				throw new ArgumentNullException(nameof(ninnelContext));

			ninnelInletStationType = this.Configuration.InletStationConfiguration.GetComponentType();
			ninnelOutletStationType = this.Configuration.OutletStationConfiguration.GetComponentType();
			middlewareConfigTypeMappings = this.Configuration
				.IntermediateStationConfigurations
				.ToDictionary(c => (IUnknownNinnelConfiguration)c,
					c => c.GetComponentType());

			if ((object)ninnelInletStationType == null)
				throw new InvalidOperationException(nameof(ninnelInletStationType));

			if ((object)ninnelOutletStationType == null)
				throw new InvalidOperationException(nameof(ninnelOutletStationType));

			// TODO: fix this
			ninnelInletStation = new DefaultComponentFactory().CreateNinnelComponent<INinnelInletStation>(AssemblyDomain.Default.DependencyManager, ninnelInletStationType, this.Configuration.Parent.ComponentAutoWire ?? false);

			if ((object)ninnelInletStation == null)
				throw new NinnelException(nameof(ninnelInletStation));

			// TODO: fix this
			ninnelOutletStation = new DefaultComponentFactory().CreateNinnelComponent<INinnelOutletStation>(AssemblyDomain.Default.DependencyManager, ninnelOutletStationType, this.Configuration.Parent.ComponentAutoWire ?? false);

			if ((object)ninnelOutletStation == null)
				throw new InvalidOperationException(nameof(ninnelOutletStation));

			await using (ninnelInletStation)
			{
				ninnelInletStation.Configuration = this.Configuration.InletStationConfiguration;
				await ninnelInletStation.CreateAsync(cancellationToken);

				await using (ninnelOutletStation)
				{
					NinnelStationFrame ninnelStationFrame;
					RecordConfiguration recordConfiguration;

					AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> process;
					IAsyncNinnelMiddlewareBuilder<NinnelStationFrame, IAsyncNinnelStream> processorBuilder;
					//INinnelMiddlewareBuilder<object, ILifecycle> processorBuilder0;

					ninnelOutletStation.Configuration = this.Configuration.OutletStationConfiguration;
					await ninnelOutletStation.CreateAsync(cancellationToken);

					recordConfiguration = this.Configuration.RecordConfiguration ?? new RecordConfiguration();

					ninnelStationFrame = new NinnelStationFrame(ninnelContext, recordConfiguration);

					await ninnelInletStation.PreExecuteAsync(ninnelStationFrame, cancellationToken);
					await ninnelOutletStation.PreExecuteAsync(ninnelStationFrame, cancellationToken);

					// --
					processorBuilder = new AsyncNinnelMiddlewareBuilder<NinnelStationFrame, IAsyncNinnelStream>();
					//processorBuilder0 = new NinnelMiddlewareBuilder<object, ILifecycle>();

					if (this.Configuration.DemoMode ?? false)
					{
						// object instance
						INinnelIntermediateStation<_DemoSpecification> intermediateStation = new _DemoIntermediateStation(null);
						((IConfigurable<IUnknownNinnelConfiguration<_DemoSpecification>>)intermediateStation)
							.Configuration = new UnknownNinnelConfiguration<_DemoSpecification>(new UnknownNinnelConfiguration(new Dictionary<string, object>(), typeof(_DemoSpecification)));
						
						await intermediateStation.CreateAsync(cancellationToken);
						processorBuilder.WithAsync<NinnelStationFrame, IAsyncNinnelStream, IUnknownNinnelConfiguration<_DemoSpecification>>(intermediateStation);

						// regular method
						processorBuilder.UseAsync(this._DemoMiddlewareMethodAsync);

						// local method
						AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> _demoMiddleware(AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> _next)
						{
							async ValueTask<IAsyncNinnelStream> _demoProcessor(NinnelStationFrame _data, IAsyncNinnelStream _target, CancellationToken _cancellationToken = default)
							{
								try
								{
									await Console.Out.WriteLineAsync("DEMO: local method BEFORE(async)");
									var retval = (object)_next != null ? await _next(_data, _target, _cancellationToken) : _target;
									await Console.Out.WriteLineAsync("DEMO: local method AFTER(async)");
									return retval;
								}
								catch (Exception ex)
								{
									throw new NinnelException(string.Format("The demo local method middleware failed (see inner exception)."), ex);
								}
							}

							return _demoProcessor;
						}

						processorBuilder.UseAsync(_demoMiddleware);

						// lambda expression
						processorBuilder.UseAsync(next =>
											{
												return async (_data, _target, _cancellationToken) =>
														{
															try
															{
																await Console.Out.WriteLineAsync("DEMO: lambda expression BEFORE(async)");
																var retval = (object)next != null ? await next(_data, _target, _cancellationToken) : _target;
																await Console.Out.WriteLineAsync("DEMO: lambda expression AFTER(async)");
																return retval;
															}
															catch (Exception ex)
															{
																throw new NinnelException(string.Format("The demo lambda expression middleware failed (see inner exception)."), ex);
															}
														};
											});
					}
					
					// by processor class (reflection)
					foreach (KeyValuePair<IUnknownNinnelConfiguration, Type> middlewareConfigTypeMapping in middlewareConfigTypeMappings)
					{
						if ((object)middlewareConfigTypeMapping.Key == null)
							throw new InvalidOperationException(nameof(middlewareConfigTypeMapping.Key));

						if ((object)middlewareConfigTypeMapping.Value == null)
							throw new InvalidOperationException(nameof(middlewareConfigTypeMapping.Value));

						// here is the issue:
						processorBuilder.FromAsync(middlewareConfigTypeMapping.Value, middlewareConfigTypeMapping.Key, this.Configuration.Parent.ComponentAutoWire ?? true, null, true);
					}

					process = processorBuilder.BuildAsync(cancellationToken);

					//var process0 = processorBuilder0.Build();

					// --

					// disposal outer-most channel
					asyncNinnelStream = await ninnelInletStation.InjectAsync(ninnelStationFrame, cancellationToken);

					await using (IAsyncNinnelStream _ninnelStream = asyncNinnelStream)
					{
						if ((object)process != null)
							asyncNinnelStream = await process(ninnelStationFrame, asyncNinnelStream, cancellationToken);

						await ninnelOutletStation.DeliverAsync(ninnelStationFrame, asyncNinnelStream, cancellationToken);
					}
					
					await ninnelOutletStation.PostExecuteAsync(ninnelStationFrame, cancellationToken);
					await ninnelInletStation.PostExecuteAsync(ninnelStationFrame, cancellationToken);
				}
			}

			return 0;
		}
		
		private AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> _DemoMiddlewareMethodAsync(AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> next)
		{
			AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> retval;
			retval = AsyncNinnelMiddlewareClosure<NinnelStationFrame, IAsyncNinnelStream>.GetNinnelMiddlewareChain(this._DemoMiddlewareMethodAsync, next);
			return retval;
		}

		private async ValueTask<IAsyncNinnelStream> _DemoMiddlewareMethodAsync(NinnelStationFrame data, IAsyncNinnelStream target, AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> next, CancellationToken cancellationToken = default)
		{
			try
			{
				IAsyncNinnelStream newAsyncNinnelStream;

				if ((object)data.NinnelContext == null)
					throw new NinnelException(nameof(data.NinnelContext));

				if ((object)data.RecordConfiguration == null)
					throw new NinnelException(nameof(data.RecordConfiguration));

				await Console.Out.WriteLineAsync("DEMO: regular method BEFORE(async)");

				if ((object)next != null)
					newAsyncNinnelStream = await next(data, target, cancellationToken);
				else
					newAsyncNinnelStream = target;

				await Console.Out.WriteLineAsync("DEMO: regular method AFTER(async)");

				return newAsyncNinnelStream;
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The regular method middleware failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}
#endif