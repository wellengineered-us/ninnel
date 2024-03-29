/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Context
{
	public partial interface INinnelContext
		: INinnelComponent<PipelineConfiguration>,
			INinnelStreamFactory,
			INinnelProductFactory
	{
		#region Properties/Indexers/Events

		IDictionary<string, object> GlobalState
		{
			get;
		}

		IDictionary<INinnelComponent0, IDictionary<string, object>> LocalState
		{
			get;
		}

		#endregion
	}
}