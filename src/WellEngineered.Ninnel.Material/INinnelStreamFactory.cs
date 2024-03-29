/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Siobhan.Primitives;

namespace WellEngineered.Ninnel.Material
{
	public partial interface INinnelStreamFactory
		: INinnelComponent0
	{
		#region Methods/Operators

		INinnelStream CreateEmptyStream();

		INinnelStream CreateStream(ILifecycleEnumerable<INinnelProduct> ninnelRecords);

		#endregion
	}
}