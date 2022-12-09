/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Middleware;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public partial interface INinnelIntermediateStation<TNinnelSpecification>
		: INinnelIntermediateStation,
			INinnelMiddleware<NinnelStationFrame, INinnelStream, IUnknownNinnelConfiguration<TNinnelSpecification>>,
			
			/* THIS IS ABSOLUTELY REQUIRED AND THERE IS NO KNOWN SOLUTION TO AVOID AMBIGUOUS WARNING */
			INinnelMiddleware<NinnelStationFrame, INinnelStream, IUnknownNinnelConfiguration>,
			INinnelStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
	}
}