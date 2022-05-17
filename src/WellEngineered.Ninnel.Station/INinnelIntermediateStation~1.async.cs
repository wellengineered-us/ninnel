/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;

using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public partial interface INinnelIntermediateStation<TNinnelSpecification>
		: INinnelStation, //INinnelIntermediateStation,
			INinnelStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
	}
}
#endif