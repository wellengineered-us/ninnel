/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;
using WellEngineered.Siobhan.Model;
using WellEngineered.Siobhan.Primitives;

namespace WellEngineered.Ninnel.Hosting.Tool.Defaults
{
	public sealed partial class DefaultToolContext
		: NinnelToolContext
	{
		#region Methods/Operators

		protected override ValueTask<INinnelProduct> CoreCreateEmptyProductAsync(CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override IAsyncNinnelStream CoreCreateEmptyStreamAsync(CancellationToken cancellationToken = default)
		{
			return null;
		}

		protected override ValueTask<INinnelProduct> CoreCreateProductAsync(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic, ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override IAsyncNinnelStream CoreCreateStreamAsync(IAsyncLifecycleEnumerable<INinnelProduct> ninnelRecords, CancellationToken cancellationToken = default)
		{
			return null;
		}

		#endregion
	}
}
#endif