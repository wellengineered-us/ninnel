/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Middleware
{
	public sealed class NinnelMiddlewareBuilder<TData, TComponent>
		: INinnelMiddlewareBuilder<TData, TComponent>
		where TComponent : ILifecycle
	{
		#region Constructors/Destructors

		public NinnelMiddlewareBuilder()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly IList<NinnelMiddlewareChainDelegate<NinnelMiddlewareDelegate<TData, TComponent>, NinnelMiddlewareDelegate<TData, TComponent>>> components = new List<NinnelMiddlewareChainDelegate<NinnelMiddlewareDelegate<TData, TComponent>, NinnelMiddlewareDelegate<TData, TComponent>>>();

		#endregion

		#region Properties/Indexers/Events

		private IList<NinnelMiddlewareChainDelegate<NinnelMiddlewareDelegate<TData, TComponent>, NinnelMiddlewareDelegate<TData, TComponent>>> Components
		{
			get
			{
				return this.components;
			}
		}

		#endregion

		#region Methods/Operators

		public NinnelMiddlewareDelegate<TData, TComponent> Build()
		{
			NinnelMiddlewareDelegate<TData, TComponent> transform = (data, target) => target; // simply return original target unmodified

			// REVERSE LIST - LIFO order
			foreach (NinnelMiddlewareChainDelegate<NinnelMiddlewareDelegate<TData, TComponent>, NinnelMiddlewareDelegate<TData, TComponent>> component in this.Components.Reverse())
			{
				if ((object)component == null)
					continue;

				transform = component.Invoke(transform);
			}

			return transform;
		}

		public INinnelMiddlewareBuilder<TData, TComponent> Use(NinnelMiddlewareChainDelegate<NinnelMiddlewareDelegate<TData, TComponent>, NinnelMiddlewareDelegate<TData, TComponent>> ninnelMiddleware)
		{
			if (ninnelMiddleware == null)
				throw new ArgumentNullException(nameof(ninnelMiddleware));

			this.Components.Add(ninnelMiddleware);
			return this;
		}

		#endregion
	}
}