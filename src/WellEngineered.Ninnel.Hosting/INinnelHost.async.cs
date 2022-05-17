/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Hosting
{
	public partial interface INinnelHost
		: INinnelComponent0
	{
		#region Methods/Operators

		ValueTask HostAsync(CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif