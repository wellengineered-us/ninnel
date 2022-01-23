/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public partial interface INinnelOutletStation<TNinnelSpecification>
		: INinnelTerminalStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Methods/Operators

		ValueTask DeliverAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream ninnelStream, CancellationToken cancellationToken = default);

		#endregion
	}
}