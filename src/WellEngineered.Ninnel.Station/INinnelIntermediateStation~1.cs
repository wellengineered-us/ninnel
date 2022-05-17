/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Middleware;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public partial interface INinnelIntermediateStation<TNinnelSpecification>
		: INinnelStation, //INinnelIntermediateStation,
			INinnelMiddleware<NinnelStationFrame, INinnelStream, IUnknownNinnelConfiguration<TNinnelSpecification>>,
			INinnelStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
	}
}