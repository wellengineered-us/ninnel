/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Material;

namespace WellEngineered.Ninnel.Station.Minimal
{
	public partial class NullOutletStation
		: NinnelOutletStation<EmptySpecification>
	{
		#region Methods/Operators

		protected override ValueTask CoreDeliverAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream ninnelStream, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override ValueTask CorePreExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override ValueTask CorPostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			return default;
		}

		#endregion
	}
}
#endif