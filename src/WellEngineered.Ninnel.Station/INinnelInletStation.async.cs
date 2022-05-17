/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Threading;

using WellEngineered.Ninnel.Material;

namespace WellEngineered.Ninnel.Station
{
	public partial interface INinnelInletStation
		: INinnelStation
	{
		#region Methods/Operators

		IAsyncNinnelStream InjectAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif