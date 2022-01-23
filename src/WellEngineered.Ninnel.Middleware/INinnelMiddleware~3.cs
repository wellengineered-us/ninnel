/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Middleware
{
	public partial interface INinnelMiddleware<TData, TComponent, TConfiguration>
		: INinnelComponent<TConfiguration>
		where TComponent : INinnelComponent0
		where TConfiguration : class, INinnelConfiguration
	{
		#region Methods/Operators

		TComponent Process(TData data, TComponent target, NinnelMiddlewareDelegate<TData, TComponent> next);

		#endregion
	}
}