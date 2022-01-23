/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Middleware
{
	public interface IAsyncNinnelMiddlewareBuilder<TData, TComponent>
		where TComponent : INinnelComponent0
	{
		#region Methods/Operators

		AsyncNinnelMiddlewareDelegate<TData, TComponent> BuildAsync();

		IAsyncNinnelMiddlewareBuilder<TData, TComponent> UseAsync(AsyncNinnelMiddlewareChainDelegate<AsyncNinnelMiddlewareDelegate<TData, TComponent>, AsyncNinnelMiddlewareDelegate<TData, TComponent>> ninnelMiddleware);

		#endregion
	}
}