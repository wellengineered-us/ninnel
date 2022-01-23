/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Middleware;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public partial interface INinnelIntermediateStation<TNinnelSpecification>
		: INinnelStation<TNinnelSpecification>,
			INinnelMiddleware<NinnelStationFrame, INinnelStreamDuality, IUnknownNinnelConfiguration<TNinnelSpecification>>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
	}
}