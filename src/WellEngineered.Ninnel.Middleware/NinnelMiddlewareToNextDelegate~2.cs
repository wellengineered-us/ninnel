/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Middleware
{
	public delegate TComponent NinnelMiddlewareToNextDelegate<TData, TComponent>(TData data, TComponent target, NinnelMiddlewareDelegate<TData, TComponent> next)
		where TComponent : INinnelComponent0;
}