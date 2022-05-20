/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WellEngineered.Solder.Primitives;

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
						INinnelIntermediateStation<_Specification> intermediateStation = new _IntermediateStation2();
						((IConfigurable<IUnknownNinnelConfiguration<_Specification>>)intermediateStation).Configuration = new UnknownNinnelConfiguration<_Specification>(new UnknownNinnelConfiguration());
						intermediateStation.Create();
						processorBuilder.With<NinnelStationFrame, INinnelStream, IUnknownNinnelConfiguration<_Specification>>(intermediateStation);

						// regular method
						processorBuilder.Use(this.TestMiddlewareMethod);

						// local method
						NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> _middleware(NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> _next)
						{
							INinnelStream _processor(NinnelStationFrame _data, INinnelStream _target)
							{
								try
								{
									Console.WriteLine("local method BEFORE");
									var retval = (object)_next != null ? _next(_data, _target) : _target;
									Console.WriteLine("local method AFTER");
									return retval;
								}
								catch (Exception ex)
								{
									throw new NinnelException(string.Format("The local method middleware failed (see inner exception)."), ex);
								}
							}

							return _processor;
						}

						processorBuilder.Use(_middleware);

						// lambda expression
						processorBuilder.Use(next =>
											{
												return (_data, _target) =>
														{
															try
															{
																Console.WriteLine("lambda expression BEFORE");
																var retval = (object)next != null ? next(_data, _target) : _target;
																Console.WriteLine("lambda expression AFTER");
																return retval;
															}
															catch (Exception ex)
															{
																throw new NinnelException(string.Format("The lambda expression middleware failed (see inner exception)."), ex);
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
			try
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
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The regular method middleware failed (see inner exception)."), ex);
			}
		}

		#endregion
	}

	public class _IntermediateStation2 : NinnelIntermediateStation<_Specification>
	{
		#region Methods/Operators

		protected override ValueTask<IAsyncNinnelStream> CoreProcessAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream asyncNinnelStream, AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> next, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override INinnelStream CoreProcess(NinnelStationFrame data, INinnelStream target, NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> next)
		{
			INinnelStream newNinnelStream;

			if ((object)data.NinnelContext == null)
				throw new NinnelException(nameof(data.NinnelContext));

			if ((object)data.RecordConfiguration == null)
				throw new NinnelException(nameof(data.RecordConfiguration));

			Console.WriteLine("INTERMEDIATE STATION object instance BEFORE");

			if ((object)next != null)
				newNinnelStream = next(data, target);
			else
				newNinnelStream = target;

			Console.WriteLine("INTERMEDIATE STATION object instance AFTER");

			return newNinnelStream;
		}

		#endregion

		protected override IUnknownSolderConfiguration<_Specification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration)
		{
			return new UnknownNinnelConfiguration<_Specification>(untypedUnknownSolderConfiguration);
		}

		protected override ValueTask CorePreExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override ValueTask CorPostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override void CorePostExecute(NinnelStationFrame ninnelStationFrame)
		{
		}

		protected override void CorePreExecute(NinnelStationFrame ninnelStationFrame)
		{
		}
	}
	
	public class _IntermediateStation : NinnelIntermediateStation<_Specification>
	{
		#region Methods/Operators

		protected override ValueTask<IAsyncNinnelStream> CoreProcessAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream asyncNinnelStream, AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> next, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override INinnelStream CoreProcess(NinnelStationFrame data, INinnelStream target, NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> next)
		{
			INinnelStream newNinnelStream;

			if ((object)data.NinnelContext == null)
				throw new NinnelException(nameof(data.NinnelContext));

			if ((object)data.RecordConfiguration == null)
				throw new NinnelException(nameof(data.RecordConfiguration));

			Console.WriteLine("INTERMEDIATE STATION BEFORE");

			if ((object)next != null)
				newNinnelStream = next(data, target);
			else
				newNinnelStream = target;

			Console.WriteLine("INTERMEDIATE STATION AFTER");

			return newNinnelStream;
		}

		#endregion

		protected override IUnknownSolderConfiguration<_Specification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration)
		{
			return new UnknownNinnelConfiguration<_Specification>(untypedUnknownSolderConfiguration);
		}

		protected override ValueTask CorePreExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override ValueTask CorPostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override void CorePostExecute(NinnelStationFrame ninnelStationFrame)
		{
		}

		protected override void CorePreExecute(NinnelStationFrame ninnelStationFrame)
		{
		}
	}
	
	public sealed partial class _Specification
		: NinnelSpecification
	{
		#region Constructors/Destructors

		public _Specification()
		{
		}

		#endregion

		#region Fields/Constants

		private string __;

		#endregion

		#region Properties/Indexers/Events

		public string _
		{
			get
			{
				return this.__;
			}
			set
			{
				this.__ = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			if (string.IsNullOrWhiteSpace(this._))
				yield return new Message(String.Empty, string.Format("Specification requires property '{0}' to be set to any non-whitespace value.", nameof(this.__)), Severity.Error);
		}
		
		protected override async IAsyncEnumerable<IMessage> CoreValidateAsync(object context, [EnumeratorCancellation] CancellationToken cancellationToken = new CancellationToken())
		{
			if (string.IsNullOrWhiteSpace(this._))
				yield return new Message(String.Empty, string.Format("Specification requires property '{0}' to be set to any non-whitespace value.", nameof(this.__)), Severity.Error);

			await Task.CompletedTask;
		}

		#endregion
	}
}