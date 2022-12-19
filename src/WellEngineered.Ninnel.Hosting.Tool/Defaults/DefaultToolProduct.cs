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
	public sealed partial class DefaultToolProduct
		: NinnelProduct
	{
		#region Constructors/Destructors

		public DefaultToolProduct(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic,
			ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock)
			: base(schema, payload, topic, partition, offset, clock)
		{
		}

		#endregion

		#region Methods/Operators

		#endregion
	}
}