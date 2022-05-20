/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Component;
using WellEngineered.Solder.Injection;

namespace WellEngineered.Ninnel.Primitives.Component
{
	public partial interface INinnelComponentFactory
		: ISolderComponent0
	{
		#region Methods/Operators

		TNinnelComponent CreateNinnelComponent<TNinnelComponent>(IDependencyManager dependencyManager, Type ninnelComponentType, bool autoWire, string selectorKey = null, bool throwOnError = true)
			where TNinnelComponent : INinnelComponent0;

		#endregion
	}
}