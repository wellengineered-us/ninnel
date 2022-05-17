/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Ninnel.Material;

namespace WellEngineered.Ninnel.Station
{
	public partial interface INinnelInletStation
		: INinnelStation
	{
		#region Methods/Operators

		INinnelStream Inject(NinnelStationFrame ninnelStationFrame);

		#endregion
	}
}