/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;

namespace WellEngineered.Ninnel.Station
{
	public partial interface INinnelOutletStation
		: INinnelStation
	{
		#region Methods/Operators

		ValueTask DeliverAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream ninnelStream, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif