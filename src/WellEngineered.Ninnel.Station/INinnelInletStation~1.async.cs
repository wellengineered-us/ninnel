/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public partial interface INinnelInletStation<TNinnelSpecification>
		: INinnelTerminalStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Methods/Operators

		IAsyncNinnelStream InjectAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default);

		#endregion
	}
}