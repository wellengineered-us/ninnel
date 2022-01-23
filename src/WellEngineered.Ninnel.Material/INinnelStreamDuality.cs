/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Material
{
	public interface INinnelStreamDuality : INinnelComponent0, INinnelStream, IAsyncNinnelStream
	{
	}
}