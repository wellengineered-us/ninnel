/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Middleware
{
	public interface INinnelMiddleware<TData, TComponent, TConfiguration>
		: /*INinnelMiddleware,*/
			INinnelComponent<TConfiguration>
		where TComponent : ILifecycle
		where TConfiguration : class, INinnelConfiguration
	{
		#region Methods/Operators

		TComponent Process(TData data, TComponent target, NinnelMiddlewareDelegate<TData, TComponent> next);

		#endregion
	}
}