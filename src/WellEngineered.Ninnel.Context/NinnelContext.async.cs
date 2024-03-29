/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

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
		#region Methods/Operators

		protected abstract ValueTask<INinnelProduct> CoreCreateEmptyProductAsync(CancellationToken cancellationToken = default);

		protected abstract IAsyncNinnelStream CoreCreateEmptyStreamAsync(CancellationToken cancellationToken = default);

		protected abstract ValueTask<INinnelProduct> CoreCreateProductAsync(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic, ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock, CancellationToken cancellationToken = default);

		protected abstract IAsyncNinnelStream CoreCreateStreamAsync(IAsyncLifecycleEnumerable<INinnelProduct> ninnelRecords, CancellationToken cancellationToken = default);

		public ValueTask<INinnelProduct> CreateEmptyProductAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				return this.CoreCreateEmptyProductAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The context failed (see inner exception)."), ex);
			}
		}

		public IAsyncNinnelStream CreateEmptyStreamAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				return this.CoreCreateEmptyStreamAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The context failed (see inner exception)."), ex);
			}
		}

		public ValueTask<INinnelProduct> CreateProductAsync(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic, ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock, CancellationToken cancellationToken = default)
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
				return this.CoreCreateProductAsync(schema, payload, topic, partition, offset, clock, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The context failed (see inner exception)."), ex);
			}
		}

		public IAsyncNinnelStream CreateStreamAsync(IAsyncLifecycleEnumerable<INinnelProduct> ninnelRecords, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelRecords == null)
				throw new ArgumentNullException(nameof(ninnelRecords));

			try
			{
				return this.CoreCreateStreamAsync(ninnelRecords, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The context failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}
#endif