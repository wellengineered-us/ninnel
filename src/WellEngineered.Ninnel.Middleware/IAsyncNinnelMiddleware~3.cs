/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Middleware
{
	public interface IAsyncNinnelMiddleware<TData, TComponent, TConfiguration>
		: /*IAsyncNinnelMiddleware,*/
			INinnelComponent<TConfiguration>
		where TComponent : IAsyncLifecycle
		where TConfiguration : class, INinnelConfiguration
	{
		#region Methods/Operators

		ValueTask<TComponent> ProcessAsync(TData data, TComponent target, AsyncNinnelMiddlewareDelegate<TData, TComponent> next, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif