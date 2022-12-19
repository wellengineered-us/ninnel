/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Material;
using WellEngineered.Siobhan.Model;
using WellEngineered.Siobhan.Primitives;

namespace WellEngineered.Ninnel.Hosting.Tool.Defaults
{
	public sealed partial class DefaultToolStream
		: NinnelStream
	{
		#region Constructors/Destructors

		public DefaultToolStream(ILifecycleEnumerable<INinnelProduct> ninnelRecords)
			: base(ninnelRecords)
		{
		}

		#endregion

		#region Methods/Operators

		#endregion
	}
}