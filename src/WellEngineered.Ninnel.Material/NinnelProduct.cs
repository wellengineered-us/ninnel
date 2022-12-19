/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Siobhan.Model;

namespace WellEngineered.Ninnel.Material
{
	public abstract class NinnelProduct
		: NinnelComponent0,
			INinnelProduct
	{
		#region Constructors/Destructors

		protected NinnelProduct(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic,
			ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock)
		{
			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			if ((object)payload == null)
				throw new ArgumentNullException(nameof(payload));

			if ((object)topic == null)
				throw new ArgumentNullException(nameof(topic));

			if ((object)partition == null)
				throw new ArgumentNullException(nameof(partition));

			if ((object)offset == null)
				throw new ArgumentNullException(nameof(offset));

			if ((object)clock == null)
				throw new ArgumentNullException(nameof(clock));

			this.schema = schema;
			this.payload = payload;
			this.topic = topic;
			this.partition = partition;
			this.offset = offset;
			this.clock = clock;
		}

		#endregion

		#region Fields/Constants

		private readonly ISiobhanClock clock;
		private readonly ISiobhanOffset offset;
		private readonly ISiobhanPartition partition;
		private readonly ISiobhanPayload payload;
		private readonly ISiobhanSchema schema;
		private readonly ISiobhanTopic topic;

		#endregion

		#region Properties/Indexers/Events

		public ISiobhanClock Clock
		{
			get
			{
				return this.clock;
			}
		}

		public ISiobhanOffset Offset
		{
			get
			{
				return this.offset;
			}
		}

		public ISiobhanPartition Partition
		{
			get
			{
				return this.partition;
			}
		}

		public ISiobhanPayload Payload
		{
			get
			{
				return this.payload;
			}
		}

		public ISiobhanSchema Schema
		{
			get
			{
				return this.schema;
			}
		}

		public ISiobhanTopic Topic
		{
			get
			{
				return this.topic;
			}
		}

		public long ClockRelativeIndex
		{
			set
			{
				this.Clock.RelativeIndex = value;
			}
		}

		#endregion
	}
}