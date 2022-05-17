/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Middleware
{
	/*public abstract partial class NinnelMiddleware
		: NinnelComponent1,
			INinnelMiddleware
	{
		protected NinnelMiddleware()
		{
		}
		
		private Type configurationType;

		public override Type ConfigurationType
		{
			get
			{
				return this.configurationType;
			}
		}

		public ILifecycle Process(object data, ILifecycle target, NinnelMiddlewareDelegate next)
		{
			//if ((object)data == null)
			//throw new ArgumentNullException(nameof(data));

			//if ((object)target == null)
			//throw new ArgumentNullException(nameof(target));
			
			//if ((object)next == null)
			//throw new ArgumentNullException(nameof(next));

			try
			{
				return this.CoreProcess0(data, target, next);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The middleware(0) failed (see inner exception)."), ex);
			}
		}

		protected abstract ILifecycle CoreProcess0(object data, ILifecycle target, NinnelMiddlewareDelegate next);
	}*/

	public abstract partial class NinnelMiddleware<TData, TComponent, TConfiguration>
		: NinnelComponent1, /*NinnelMiddleware,*/
			INinnelMiddleware<TData, TComponent, TConfiguration>
		where TComponent : ILifecycle
		where TConfiguration : class, INinnelConfiguration
	{
		#region Constructors/Destructors

		protected NinnelMiddleware()
		{
		}

		#endregion

		#region Properties/Indexers/Events

		public override Type ConfigurationType
		{
			get
			{
				return typeof(TConfiguration);
			}
		}

		TConfiguration IConfigurable<TConfiguration>.Configuration
		{
			get
			{
				return base.Configuration as TConfiguration;
			}
			set
			{
				base.Configuration = value;
			}
		}

		#endregion

		#region Methods/Operators

		/*protected override ILifecycle CoreProcess0(object data, ILifecycle target, NinnelMiddlewareDelegate next)
		{
			return null;
		}*/

		protected abstract TComponent CoreProcess(TData data, TComponent target, NinnelMiddlewareDelegate<TData, TComponent> next);

		TComponent INinnelMiddleware<TData, TComponent, TConfiguration>.Process(TData data, TComponent target, NinnelMiddlewareDelegate<TData, TComponent> next)
		{
			//if ((object)data == null)
			//throw new ArgumentNullException(nameof(data));

			//if ((object)target == null)
			//throw new ArgumentNullException(nameof(target));

			//if ((object)target == null)
			//throw new ArgumentNullException(nameof(target));

			try
			{
				return this.CoreProcess(data, target, next);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The middleware failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}