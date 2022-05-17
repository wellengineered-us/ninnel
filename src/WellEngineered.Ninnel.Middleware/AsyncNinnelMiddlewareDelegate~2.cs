/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Middleware
{
	public delegate ValueTask<TComponent> AsyncNinnelMiddlewareDelegate<in TData, TComponent>(TData data, TComponent target)
		where TComponent : IAsyncLifecycle;
}
#endif