/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public partial interface INinnelStation<TNinnelSpecification>
		: INinnelComponent<IUnknownNinnelConfiguration<TNinnelSpecification>, TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Methods/Operators

		ValueTask PostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default);

		ValueTask PreExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default);

		#endregion
	}
}