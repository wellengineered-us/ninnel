/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Transport
{
	public interface INinnelTransport<TNinnelConfiguration>
		: INinnelComponent<TNinnelConfiguration>
		where TNinnelConfiguration : class, INinnelConfiguration
	{
	}
}