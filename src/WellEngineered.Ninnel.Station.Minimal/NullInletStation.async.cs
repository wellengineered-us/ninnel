/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Siobhan.Model;
using WellEngineered.Siobhan.Primitives;

namespace WellEngineered.Ninnel.Station.Minimal
{
	public partial class NullInletStation
		: NinnelInletStation<EmptySpecification>
	{
		#region Methods/Operators

		private static async IAsyncEnumerable<ISiobhanPayload> GetRandomPayloadsAsync(ISiobhanSchema schema, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			ISiobhanPayload payload;
			ISiobhanField[] fields;

			long recordCount;

			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			fields = schema.Fields.Values.ToArray();

			lock (syncLock)
				recordCount = Random.Next(MIN_RECORD_COUNT, MAX_RECORD_COUNT);

			recordCount = MAX_RECORD_COUNT;

			if (recordCount == INVALID_RANDOM_VALUE)
				throw new NinnelException(nameof(INVALID_RANDOM_VALUE));

			for (long recordIndex = 0; recordIndex < recordCount; recordIndex++)
			{
				payload = new SiobhanPayload();

				for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
				{
					lock (syncLock)
					{
						if (fields[fieldIndex].IsFieldKeyComponent)
							payload.Add(fields[fieldIndex].FieldName, Guid.NewGuid());
						else
							payload.Add(fields[fieldIndex].FieldName, Random.NextDouble());
					}
				}

				yield return payload;
			}
		}

		protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = new CancellationToken())
		{
			// do nothing
			return base.CoreCreateAsync(creating, cancellationToken);
		}

		protected override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = new CancellationToken())
		{
			// do nothing
			return base.CoreDisposeAsync(disposing, cancellationToken);
		}

		protected async override ValueTask<IAsyncNinnelStream> CoreInjectAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			IAsyncNinnelStream stream;
			ISiobhanSchema schema;

			IAsyncEnumerable<ISiobhanPayload> payloads;

			if ((object)ninnelStationFrame.NinnelContext == null)
				throw new NinnelException(nameof(ninnelStationFrame.NinnelContext));

			if ((object)ninnelStationFrame.RecordConfiguration == null)
				throw new NinnelException(nameof(ninnelStationFrame.RecordConfiguration));

			if (!ninnelStationFrame.NinnelContext.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				ninnelStationFrame.NinnelContext.LocalState.Add(this, localState);
			}

			schema = localState[Constants.ContextComponentScopedSchema] as ISiobhanSchema;

			if ((object)schema == null)
				throw new NinnelException(nameof(schema));

			payloads = GetRandomPayloadsAsync(schema, cancellationToken);

			if ((object)payloads == null)
				throw new NinnelException(nameof(payloads));

			var records = payloads.Select(p =>
											ninnelStationFrame.NinnelContext
												.CreateProduct(schema, p, SiobhanTopic.None, SiobhanPartition.None,
													SiobhanOffset.Default, SiobhanClock.GetNowAt()))
				.ToAsyncLifecycleEnumerable();

			stream = ninnelStationFrame.NinnelContext.CreateStreamAsync(records);

			return stream;
		}

		protected async override ValueTask CorePreExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			ISiobhanSchema schema;

			if ((object)ninnelStationFrame.NinnelContext == null)
				throw new NinnelException(nameof(ninnelStationFrame.NinnelContext));

			if ((object)ninnelStationFrame.RecordConfiguration == null)
				throw new NinnelException(nameof(ninnelStationFrame.RecordConfiguration));

			if (!ninnelStationFrame.NinnelContext.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				ninnelStationFrame.NinnelContext.LocalState.Add(this, localState);
			}

			schema = GetRandomSchema();

			if ((object)schema == null)
				throw new NinnelException(nameof(schema));

			localState.Add(Constants.ContextComponentScopedSchema, schema);

			await Task.CompletedTask;
		}

		protected async override ValueTask CorPostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelStationFrame.NinnelContext == null)
				throw new NinnelException(nameof(ninnelStationFrame.NinnelContext));

			if ((object)ninnelStationFrame.RecordConfiguration == null)
				throw new NinnelException(nameof(ninnelStationFrame.RecordConfiguration));

			if (ninnelStationFrame.NinnelContext.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState.Clear();
				ninnelStationFrame.NinnelContext.LocalState.Remove(this);
			}

			await Task.CompletedTask;
		}

		#endregion
	}
}
#endif