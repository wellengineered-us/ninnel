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

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Middleware
{
	public sealed class AsyncNinnelMiddlewareBuilder<TData, TComponent>
		: IAsyncNinnelMiddlewareBuilder<TData, TComponent>
		where TComponent : IAsyncLifecycle
	{
		#region Constructors/Destructors

		public AsyncNinnelMiddlewareBuilder()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly IList<AsyncNinnelMiddlewareChainDelegate<AsyncNinnelMiddlewareDelegate<TData, TComponent>, AsyncNinnelMiddlewareDelegate<TData, TComponent>>> asyncComponents = new List<AsyncNinnelMiddlewareChainDelegate<AsyncNinnelMiddlewareDelegate<TData, TComponent>, AsyncNinnelMiddlewareDelegate<TData, TComponent>>>();

		#endregion

		#region Properties/Indexers/Events

		private IList<AsyncNinnelMiddlewareChainDelegate<AsyncNinnelMiddlewareDelegate<TData, TComponent>, AsyncNinnelMiddlewareDelegate<TData, TComponent>>> AsyncComponents
		{
			get
			{
				return this.asyncComponents;
			}
		}

		#endregion

		#region Methods/Operators

		public AsyncNinnelMiddlewareDelegate<TData, TComponent> BuildAsync(CancellationToken cancellationToken = default)
		{
			AsyncNinnelMiddlewareDelegate<TData, TComponent> asyncTransform = async (data, target, ct) =>
																			{
																				await Task.CompletedTask;
																				return target;
																			}; // simply return original target unmodified

			// REVERSE LIST - LIFO order
			foreach (AsyncNinnelMiddlewareChainDelegate<AsyncNinnelMiddlewareDelegate<TData, TComponent>, AsyncNinnelMiddlewareDelegate<TData, TComponent>> asyncComponents in this.AsyncComponents.Reverse())
			{
				if ((object)asyncComponents == null)
					continue;

				asyncTransform = asyncComponents(asyncTransform);
			}

			return asyncTransform;
		}

		public IAsyncNinnelMiddlewareBuilder<TData, TComponent> UseAsync(AsyncNinnelMiddlewareChainDelegate<AsyncNinnelMiddlewareDelegate<TData, TComponent>, AsyncNinnelMiddlewareDelegate<TData, TComponent>> ninnelMiddleware, CancellationToken cancellationToken = default)
		{
			if (ninnelMiddleware == null)
				throw new ArgumentNullException(nameof(ninnelMiddleware));

			this.AsyncComponents.Add(ninnelMiddleware);
			return this;
		}

		#endregion
	}
}
#endif