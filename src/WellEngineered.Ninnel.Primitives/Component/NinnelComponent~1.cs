/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Component;

namespace WellEngineered.Ninnel.Primitives.Component
{
	public abstract class NinnelComponent<TNinnelConfiguration>
		: SolderComponent<TNinnelConfiguration>,
			INinnelComponent<TNinnelConfiguration>
		where TNinnelConfiguration : class, INinnelConfiguration
	{
	}
}