/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Threading;
using System.Threading.Tasks;

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

		ValueTask<TComponent> ProcessAsync(TData data, TComponent target, AsyncNinnelMiddlewareDelegate<TData, TComponent> next, CancellationToken cancellationToken = default);

		#endregion
	}
}