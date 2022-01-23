/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;
using WellEngineered.Siobhan.Model;
using WellEngineered.Siobhan.Primitives;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public sealed class DefaultToolContext
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
			return null;
		}

		protected override ValueTask<INinnelProduct> CoreCreateEmptyProductAsync(CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override INinnelStream CoreCreateEmptyStream()
		{
			return null;
		}

		protected override IAsyncNinnelStream CoreCreateEmptyStreamAsync(CancellationToken cancellationToken = default)
		{
			return null;
		}

		protected override INinnelProduct CoreCreateProduct(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic, ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock)
		{
			return null;
		}

		protected override ValueTask<INinnelProduct> CoreCreateProductAsync(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic, ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override INinnelStream CoreCreateStream(ILifecycleEnumerable<INinnelProduct> ninnelRecords)
		{
			return null;
		}

		protected override IAsyncNinnelStream CoreCreateStreamAsync(IAsyncLifecycleEnumerable<INinnelProduct> ninnelRecords, CancellationToken cancellationToken = default)
		{
			return null;
		}

		#endregion
	}
}