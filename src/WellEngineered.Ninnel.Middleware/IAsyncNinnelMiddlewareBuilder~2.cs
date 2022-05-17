/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Middleware
{
	public interface IAsyncNinnelMiddlewareBuilder<TData, TComponent>
		where TComponent : IAsyncLifecycle
	{
		#region Methods/Operators

		AsyncNinnelMiddlewareDelegate<TData, TComponent> BuildAsync();

		IAsyncNinnelMiddlewareBuilder<TData, TComponent> UseAsync(AsyncNinnelMiddlewareChainDelegate<AsyncNinnelMiddlewareDelegate<TData, TComponent>, AsyncNinnelMiddlewareDelegate<TData, TComponent>> ninnelMiddleware);

		#endregion
	}
}
#endif