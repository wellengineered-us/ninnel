/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Middleware
{
	public delegate TComponent NinnelMiddlewareDelegate<in TData, TComponent>(TData data, TComponent target)
		where TComponent : ILifecycle;
}