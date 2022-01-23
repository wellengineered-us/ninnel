/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Middleware
{
	public interface INinnelMiddlewareBuilder<TData, TComponent>
		where TComponent : INinnelComponent0
	{
		#region Methods/Operators

		NinnelMiddlewareDelegate<TData, TComponent> Build();

		INinnelMiddlewareBuilder<TData, TComponent> Use(NinnelMiddlewareChainDelegate<NinnelMiddlewareDelegate<TData, TComponent>, NinnelMiddlewareDelegate<TData, TComponent>> ninnelMiddleware);

		#endregion
	}
}