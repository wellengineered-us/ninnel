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

					if (true)
					{
						// object instance
						INinnelMiddleware<NinnelStationFrame, INinnelStream, IUnknownNinnelConfiguration> middleware = new _Middleware();
						middleware.Configuration = new UnknownNinnelConfiguration();
						middleware.Create();
						processorBuilder.With(middleware);

						//INinnelMiddleware middleware0 = new _Middleware0();
						//middleware0.Configuration = new UnknownNinnelConfiguration();
						//middleware0.Create();
						//processorBuilder0.With(middleware0);

						// regular method
						processorBuilder.Use(this.TestMiddlewareMethod);

						// local method
						NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> _middleware(NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> _next)
						{
							INinnelStream _processor(NinnelStationFrame _data, INinnelStream _target)
							{
								Console.WriteLine("local method BEFORE");
								var retval = (object)_next != null ? _next(_data, _target) : _target;
								Console.WriteLine("local method AFTER");
								return retval;
							}

							return _processor;
						}

						processorBuilder.Use(_middleware);

						// lambda expression
						processorBuilder.Use(next =>
											{
												return (_data, _target) =>
														{
															Console.WriteLine("lambda expression BEFORE");
															var retval = (object)next != null ? next(_data, _target) : _target;
															Console.WriteLine("lambda expression AFTER");
															return retval;
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

						processorBuilder.From(middlewareConfigTypeMapping.Value, middlewareConfigTypeMapping.Key, this.Configuration.Parent.ComponentAutoWire ?? true);
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

						//if((object)process0 != null)
						//ninnelStream = (INinnelStream)process0(ninnelStationFrame, ninnelStream);

						ninnelOutletStation.Deliver(ninnelStationFrame, ninnelStream);
					}

					ninnelOutletStation.PostExecute(ninnelStationFrame);
					ninnelInletStation.PostExecute(ninnelStationFrame);
				}
			}

			return 0;

			// AssemblyDomain.Default.ResourceManager.Print(Guid.Empty, "Mark");
			// Thread.Sleep(4000);
			// AssemblyDomain.Default.ResourceManager.Print(Guid.Empty, "Set");
			// Thread.Sleep(2000);
			// AssemblyDomain.Default.ResourceManager.Print(Guid.Empty, "Ready");
			// Thread.Sleep(1000);
			// AssemblyDomain.Default.ResourceManager.Print(Guid.Empty, "Go");
			//
			// return 0;*/
		}

		private NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> TestMiddlewareMethod(NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> next)
		{
			NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> retval;
			retval = NinnelMiddlewareClosure<NinnelStationFrame, INinnelStream>.GetNinnelMiddlewareChain(this.TestMiddlewareMethod, next);
			return retval;
		}

		private INinnelStream TestMiddlewareMethod(NinnelStationFrame data, INinnelStream target, NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> next)
		{
			INinnelStream newNinnelStream;

			if ((object)data.NinnelContext == null)
				throw new NinnelException(nameof(data.NinnelContext));

			if ((object)data.RecordConfiguration == null)
				throw new NinnelException(nameof(data.RecordConfiguration));

			Console.WriteLine("regular method BEFORE");

			if ((object)next != null)
				newNinnelStream = next(data, target);
			else
				newNinnelStream = target;

			Console.WriteLine("regular method AFTER");

			return newNinnelStream;
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		/*private class _Middleware0 : NinnelMiddleware
		{
			protected override ILifecycle CoreProcess0(object data, ILifecycle target, NinnelMiddlewareDelegate next)
			{
				ILifecycle newNinnelStream;

				if ((object)data == null)
					throw new NinnelException(nameof(data));
			
				Console.WriteLine("object instance0 BEFORE");

				if ((object)next != null)
					newNinnelStream = next(data, target);
				else
					newNinnelStream = target;

				Console.WriteLine("object instance0 AFTER");

				return newNinnelStream;
			}
		}*/

		private class _Middleware : NinnelMiddleware<NinnelStationFrame, INinnelStream, IUnknownNinnelConfiguration>
		{
			#region Methods/Operators

			protected override INinnelStream CoreProcess(NinnelStationFrame data, INinnelStream target, NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> next)
			{
				INinnelStream newNinnelStream;

				if ((object)data.NinnelContext == null)
					throw new NinnelException(nameof(data.NinnelContext));

				if ((object)data.RecordConfiguration == null)
					throw new NinnelException(nameof(data.RecordConfiguration));

				Console.WriteLine("object instance BEFORE");

				if ((object)next != null)
					newNinnelStream = next(data, target);
				else
					newNinnelStream = target;

				Console.WriteLine("object instance AFTER");

				return newNinnelStream;
			}

			#endregion
		}

		#endregion
	}
}