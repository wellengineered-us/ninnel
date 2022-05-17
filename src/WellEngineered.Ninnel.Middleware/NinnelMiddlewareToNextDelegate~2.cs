/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Middleware
{
	public delegate TComponent NinnelMiddlewareToNextDelegate<TData, TComponent>(TData data, TComponent target, NinnelMiddlewareDelegate<TData, TComponent> next)
		where TComponent : ILifecycle;
}