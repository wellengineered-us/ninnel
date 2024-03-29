/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Siobhan.Primitives;

namespace WellEngineered.Ninnel.Material
{
	public abstract class NinnelStream
		: ForEachLifecycleYieldStateMachine<INinnelProduct, INinnelProduct>,
			INinnelStream
	{
		#region Constructors/Destructors

		protected NinnelStream(ILifecycleEnumerable<INinnelProduct> baseEnumerable)
			: base(baseEnumerable, (ix, p) => p)
		{
		}

		#endregion
	}
}