/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Component;

namespace WellEngineered.Ninnel.Primitives.Component
{
	public interface INinnelComponent<TNinnelConfiguration>
		: INinnelComponent1,
			ISolderComponent<TNinnelConfiguration>
		where TNinnelConfiguration : class, INinnelConfiguration
	{
	}
}