/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

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
		#region Constructors/Destructors

		public DefaultToolPipeline()
		{
		}

		#endregion

		#region Methods/Operators

		protected override INinnelContext CoreCloneContext(INinnelContext ninnelContext)
		{
			// for now, just return this one
			return ninnelContext;
		}

		protected override INinnelContext CoreCreateContext(Type ninnelContextType)
		{
			if ((object)ninnelContextType == null)
				throw new ArgumentNullException(nameof(ninnelContextType));

			// TODO: fix this
			return new DefaultComponentFactory().CreateNinnelComponent<INinnelContext>(AssemblyDomain.Default.DependencyManager, ninnelContextType, this.Configuration.Parent.ComponentAutoWire ?? false);
		}

		protected override long CoreExecute(INinnelContext ninnelContext)
		{
			INinnelStream ninnelStream;

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

			using (ninnelInletStation)
			{
				ninnelInletStation.Configuration = this.Configuration.InletStationConfiguration;
				ninnelInletStation.Create();

				using (ninnelOutletStation)
				{
					NinnelStationFrame ninnelStationFrame;
					RecordConfiguration recordConfiguration;

					NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> process;
					INinnelMiddlewareBuilder<NinnelStationFrame, INinnelStream> processorBuilder;
					//INinnelMiddlewareBuilder<object, ILifecycle> processorBuilder0;

					ninnelOutletStation.Configuration = this.Configuration.OutletStationConfiguration;
					ninnelOutletStation.Create();

					recordConfiguration = this.Configuration.RecordConfiguration ?? new RecordConfiguration();

					ninnelStationFrame = new NinnelStationFrame(ninnelContext, recordConfiguration);

					ninnelInletStation.PreExecute(ninnelStationFrame);
					ninnelOutletStation.PreExecute(ninnelStationFrame);

					// --
					processorBuilder = new NinnelMiddlewareBuilder<NinnelStationFrame, INinnelStream>();
					//processorBuilder0 = new NinnelMiddlewareBuilder<object, ILifecycle>();

					if (this.Configuration.DemoMode ?? false)
					{
						// object instance
						INinnelIntermediateStation<_DemoSpecification> intermediateStation = new _DemoIntermediateStation(null);
						((IConfigurable<IUnknownNinnelConfiguration<_DemoSpecification>>)intermediateStation)
							.Configuration = new UnknownNinnelConfiguration<_DemoSpecification>(new UnknownNinnelConfiguration());
						
						intermediateStation.Create();
						processorBuilder.With<NinnelStationFrame, INinnelStream, IUnknownNinnelConfiguration<_DemoSpecification>>(intermediateStation);

						// regular method
						processorBuilder.Use(this._DemoMiddlewareMethod);

						// local method
						NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> _demoMiddleware(NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> _next)
						{
							INinnelStream _demoProcessor(NinnelStationFrame _data, INinnelStream _target)
							{
								try
								{
									Console.WriteLine("DEMO: local method BEFORE");
									var retval = (object)_next != null ? _next(_data, _target) : _target;
									Console.WriteLine("DEMO: local method AFTER");
									return retval;
								}
								catch (Exception ex)
								{
									throw new NinnelException(string.Format("The demo local method middleware failed (see inner exception)."), ex);
								}
							}

							return _demoProcessor;
						}

						processorBuilder.Use(_demoMiddleware);

						// lambda expression
						processorBuilder.Use(next =>
											{
												return (_data, _target) =>
														{
															try
															{
																Console.WriteLine("DEMO: lambda expression BEFORE");
																var retval = (object)next != null ? next(_data, _target) : _target;
																Console.WriteLine("DEMO: lambda expression AFTER");
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
						processorBuilder.From(middlewareConfigTypeMapping.Value, middlewareConfigTypeMapping.Key, this.Configuration.Parent.ComponentAutoWire ?? true, null, true);
					}

					process = processorBuilder.Build();

					//var process0 = processorBuilder0.Build();

					// --

					// disposal outer-most channel
					ninnelStream = ninnelInletStation.Inject(ninnelStationFrame);

					using (INinnelStream _ninnelStream = ninnelStream)
					{
						if ((object)process != null)
							ninnelStream = process(ninnelStationFrame, ninnelStream);

						ninnelOutletStation.Deliver(ninnelStationFrame, ninnelStream);
					}

					ninnelOutletStation.PostExecute(ninnelStationFrame);
					ninnelInletStation.PostExecute(ninnelStationFrame);
				}
			}

			return 0;
		}

		private NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> _DemoMiddlewareMethod(NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> next)
		{
			NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> retval;
			retval = NinnelMiddlewareClosure<NinnelStationFrame, INinnelStream>.GetNinnelMiddlewareChain(this._DemoMiddlewareMethod, next);
			return retval;
		}

		private INinnelStream _DemoMiddlewareMethod(NinnelStationFrame data, INinnelStream target, NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> next)
		{
			try
			{
				INinnelStream newNinnelStream;

				if ((object)data.NinnelContext == null)
					throw new NinnelException(nameof(data.NinnelContext));

				if ((object)data.RecordConfiguration == null)
					throw new NinnelException(nameof(data.RecordConfiguration));

				Console.WriteLine("DEMO: regular method BEFORE");

				if ((object)next != null)
					newNinnelStream = next(data, target);
				else
					newNinnelStream = target;

				Console.WriteLine("DEMO: regular method AFTER");

				return newNinnelStream;
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The regular method middleware failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}