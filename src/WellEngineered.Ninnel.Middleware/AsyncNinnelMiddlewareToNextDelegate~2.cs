/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Middleware
{
	public delegate ValueTask<TComponent> AsyncNinnelMiddlewareToNextDelegate<TData, TComponent>(TData data, TComponent target, AsyncNinnelMiddlewareDelegate<TData, TComponent> next, CancellationToken cancellationToken)
		where TComponent : IAsyncLifecycle;
}
#endif