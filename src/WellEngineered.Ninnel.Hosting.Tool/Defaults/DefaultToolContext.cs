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
	public sealed partial class DefaultToolContext
		: NinnelToolContext
	{
		#region Constructors/Destructors

		public DefaultToolContext()
		{
		}

		#endregion

		#region Methods/Operators

		protected override INinnelProduct CoreCreateEmptyProduct()
		{
			return null; //new NinnelProduct(new SiobhanSiobhanSchemaBuilder().Schema, new SiobhanPayload(), new SiobhanTopic(), SiobhanPartition.None);
		}

		protected override INinnelStream CoreCreateEmptyStream()
		{
			return null;
		}

		protected override INinnelProduct CoreCreateProduct(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic, ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock)
		{
			return null;
		}

		protected override INinnelStream CoreCreateStream(ILifecycleEnumerable<INinnelProduct> ninnelRecords)
		{
			return null;
		}

		#endregion
	}
}