/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Middleware;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public partial interface INinnelIntermediateStation<TNinnelSpecification>
		: INinnelIntermediateStation,
			IAsyncNinnelMiddleware<NinnelStationFrame, IAsyncNinnelStream, IUnknownNinnelConfiguration<TNinnelSpecification>>,
			//IAsyncNinnelMiddleware<NinnelStationFrame, IAsyncNinnelStream, IUnknownNinnelConfiguration>,
			INinnelStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
	}
}
#endif