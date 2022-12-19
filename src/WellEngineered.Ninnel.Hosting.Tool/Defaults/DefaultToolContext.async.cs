/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Siobhan.Middleware;
using WellEngineered.Siobhan.Model;
using WellEngineered.Siobhan.Primitives;

namespace WellEngineered.Ninnel.Hosting.Tool.Defaults
{
	public sealed partial class DefaultToolContext
		: NinnelToolContext
	{
		#region Methods/Operators

		protected async override ValueTask<INinnelProduct> CoreCreateEmptyProductAsync(CancellationToken cancellationToken = default)
		{
			await Task.CompletedTask;
			
			return new DefaultToolProduct(new SiobhanSchemaBuilder().Schema,
				SiobhanPayload.FromPrimitive(null), SiobhanTopic.None, SiobhanPartition.None,
				SiobhanOffset.Default, SiobhanClock.GetNowAt());
		}

		protected override IAsyncNinnelStream CoreCreateEmptyStreamAsync(CancellationToken cancellationToken = default)
		{
			return new AsyncDefaultToolStream(GetNinnelProductsAsync()
				.ToAsyncLifecycleEnumerable());

			async IAsyncEnumerable<INinnelProduct> GetNinnelProductsAsync()
			{
				await Task.CompletedTask;
				yield break;
			}
		}

		protected async override ValueTask<INinnelProduct> CoreCreateProductAsync(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic, ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock, CancellationToken cancellationToken = default)
		{
			await Task.CompletedTask;
			
			return new DefaultToolProduct(schema, payload, topic, partition, offset, clock);
		}

		protected override IAsyncNinnelStream CoreCreateStreamAsync(IAsyncLifecycleEnumerable<INinnelProduct> ninnelRecords, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelRecords == null)
				throw new ArgumentNullException(nameof(ninnelRecords));

			ninnelRecords = ninnelRecords
				.GetWrappedAsyncEnumerable(ApplyRecordIndexAsync, cancellationToken)
				.GetWrappedAsyncEnumerable("records", LogItemAsync, this.LogMetricsAsync, cancellationToken)
				.ToAsyncLifecycleEnumerable();
			return new AsyncDefaultToolStream(ninnelRecords);
		}
		
		private static async ValueTask<INinnelProduct> ApplyRecordIndexAsync(long recordIndex, INinnelProduct record, CancellationToken cancellationToken = default)
		{
			if ((object)record == null)
				throw new ArgumentNullException(nameof(record));

			record.ClockRelativeIndex = recordIndex;
			await Task.CompletedTask;
			
			return record;
		}

		private static async ValueTask<T> LogItemAsync<T>(long itemIndex, T item, CancellationToken cancellationToken = default)
		{
			//Console.WriteLine(item.ToStringEx(null, "<null>"));
			await Task.CompletedTask;
			
			return item;
		}
		
		private async ValueTask LogMetricsAsync(string sourceLabel, long itemIndex, bool isCompleted, double rollingTiming, CancellationToken cancellationToken = default)
		{
			if (itemIndex == -1 || isCompleted)
				await Console.Out.WriteLineAsync(string.Format("{0}@{4}-{5:N}: itemIndex = {1}, isCompleted = {2}, rollingTiming = {3}", sourceLabel, itemIndex, isCompleted, rollingTiming, Environment.CurrentManagedThreadId, this.ComponentId));
		}

		#endregion
	}
}
#endif