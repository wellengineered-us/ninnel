/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Station
{
	public interface INinnelStation
		: INinnelComponent2
	{
		#region Methods/Operators

		void PostExecute(NinnelStationFrame ninnelStationFrame);

		ValueTask PostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default);

		void PreExecute(NinnelStationFrame ninnelStationFrame);

		ValueTask PreExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default);

		#endregion
	}
}