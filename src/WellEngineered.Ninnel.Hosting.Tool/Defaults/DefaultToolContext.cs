/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Siobhan.Middleware;
using WellEngineered.Siobhan.Model;
using WellEngineered.Siobhan.Primitives;
using WellEngineered.Solder.Primitives;

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
			return new DefaultToolProduct(new SiobhanSchemaBuilder().Schema,
				SiobhanPayload.FromPrimitive(null), SiobhanTopic.None, SiobhanPartition.None,
				SiobhanOffset.Default, SiobhanClock.GetNowAt());
		}

		protected override INinnelStream CoreCreateEmptyStream()
		{
			return new DefaultToolStream(GetNinnelProducts()
				.ToLifecycleEnumerable());
			
			IEnumerable<INinnelProduct> GetNinnelProducts()
			{
				yield break;
			}
		}

		protected override INinnelProduct CoreCreateProduct(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic, ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock)
		{
			return new DefaultToolProduct(schema, payload, topic, partition, offset, clock);
		}

		protected override INinnelStream CoreCreateStream(ILifecycleEnumerable<INinnelProduct> ninnelRecords)
		{
			if ((object)ninnelRecords == null)
				throw new ArgumentNullException(nameof(ninnelRecords));

			ninnelRecords = ninnelRecords
				.GetWrappedEnumerable(ApplyRecordIndex)
				.GetWrappedEnumerable("records", LogItem, this.LogMetrics)
				.ToLifecycleEnumerable();
			return new DefaultToolStream(ninnelRecords);
		}
		
		private static INinnelProduct ApplyRecordIndex(long recordIndex, INinnelProduct record)
		{
			if ((object)record == null)
				throw new ArgumentNullException(nameof(record));

			record.ClockRelativeIndex = recordIndex;
			return record;
		}

		private static T LogItem<T>(long itemIndex, T item)
		{
			//Console.WriteLine(item.ToStringEx(null, "<null>"));
			return item;
		}
		
		private void LogMetrics(string sourceLabel, long itemIndex, bool isCompleted, double rollingTiming)
		{
			if (itemIndex == -1 || isCompleted)
				Console.WriteLine("{0}@{4}-{5:N}: itemIndex = {1}, isCompleted = {2}, rollingTiming = {3}", sourceLabel, itemIndex, isCompleted, rollingTiming, Environment.CurrentManagedThreadId, this.ComponentId);
		}

		#endregion
	}
}