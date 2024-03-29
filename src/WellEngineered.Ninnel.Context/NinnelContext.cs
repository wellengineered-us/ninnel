/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Siobhan.Model;
using WellEngineered.Siobhan.Primitives;

namespace WellEngineered.Ninnel.Context
{
	public abstract partial class NinnelContext
		: NinnelComponent<PipelineConfiguration>,
			INinnelContext
	{
		#region Constructors/Destructors

		protected NinnelContext()
			: this(new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase),
				new ConcurrentDictionary<INinnelComponent0, IDictionary<string, object>>())
		{
		}

		protected NinnelContext(IDictionary<string, object> globalState, IDictionary<INinnelComponent0, IDictionary<string, object>> localState)
		{
			if ((object)globalState == null)
				throw new ArgumentNullException(nameof(globalState));

			if ((object)localState == null)
				throw new ArgumentNullException(nameof(localState));

			this.globalState = globalState;
			this.localState = localState;
		}

		#endregion

		#region Fields/Constants

		private readonly IDictionary<string, object> globalState;
		private readonly IDictionary<INinnelComponent0, IDictionary<string, object>> localState;

		#endregion

		#region Properties/Indexers/Events

		public IDictionary<string, object> GlobalState
		{
			get
			{
				return this.globalState;
			}
		}

		public IDictionary<INinnelComponent0, IDictionary<string, object>> LocalState
		{
			get
			{
				return this.localState;
			}
		}

		#endregion

		#region Methods/Operators

		protected abstract INinnelProduct CoreCreateEmptyProduct();

		protected abstract INinnelStream CoreCreateEmptyStream();

		protected abstract INinnelProduct CoreCreateProduct(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic, ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock);

		protected abstract INinnelStream CoreCreateStream(ILifecycleEnumerable<INinnelProduct> ninnelRecords);

		public INinnelProduct CreateEmptyProduct()
		{
			try
			{
				return this.CoreCreateEmptyProduct();
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The context failed (see inner exception)."), ex);
			}
		}

		public INinnelStream CreateEmptyStream()
		{
			try
			{
				return this.CoreCreateEmptyStream();
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The context failed (see inner exception)."), ex);
			}
		}

		public INinnelProduct CreateProduct(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic, ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock)
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

			try
			{
				return this.CoreCreateProduct(schema, payload, topic, partition, offset, clock);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The context failed (see inner exception)."), ex);
			}
		}

		public INinnelStream CreateStream(ILifecycleEnumerable<INinnelProduct> ninnelRecords)
		{
			if ((object)ninnelRecords == null)
				throw new ArgumentNullException(nameof(ninnelRecords));

			try
			{
				return this.CoreCreateStream(ninnelRecords);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The context failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}