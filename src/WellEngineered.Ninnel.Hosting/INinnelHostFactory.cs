/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Hosting
{
	public partial interface INinnelHostFactory
		: INinnelComponent0
	{
		#region Methods/Operators

		INinnelHost CreateHost();

		#endregion
	}
}