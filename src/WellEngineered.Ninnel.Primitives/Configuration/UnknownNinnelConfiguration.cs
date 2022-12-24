/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Solder.Component;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Primitives.Configuration
{
	public class UnknownNinnelConfiguration
		: UnknownSolderConfiguration,
			IUnknownNinnelConfiguration
	{
		#region Constructors/Destructors

		public UnknownNinnelConfiguration()
			: base(new Dictionary<string, object>(), null)
		{
		}
		
		public UnknownNinnelConfiguration(IDictionary<string, object> componentSpecificConfiguration, Type componentSpecificConfigurationType)
			: base(componentSpecificConfiguration, componentSpecificConfigurationType)
		{
		}

		#endregion
	}
}