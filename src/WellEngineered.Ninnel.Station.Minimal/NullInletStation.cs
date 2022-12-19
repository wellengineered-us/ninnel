/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Siobhan.Model;
using WellEngineered.Siobhan.Primitives;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Station.Minimal
{
	public partial class NullInletStation
		: NinnelInletStation<EmptySpecification>
	{
		#region Constructors/Destructors

		public NullInletStation()
		{
		}

		#endregion

		#region Fields/Constants

		private const string FIELD_NAME = "RandomValue_{0:00}";
		private const int INVALID_RANDOM_VALUE = 0;
		private const int MAX_FIELD_COUNT = 10;
		private const int MAX_RECORD_COUNT = 100000;
		private const int MIN_FIELD_COUNT = 1;
		private const int MIN_RECORD_COUNT = 1;
		private static readonly Random random = new Random();
		private static readonly object syncLock = new object();

		#endregion

		#region Properties/Indexers/Events

		private static Random Random
		{
			get
			{
				return random;
			}
		}

		#endregion

		#region Methods/Operators

		private static IEnumerable<ISiobhanPayload> GetRandomPayloads(ISiobhanSchema schema)
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

		private static ISiobhanSchema GetRandomSchema()
		{
			long fieldCount;
			SiobhanSchemaBuilder schemaBuilder;

			schemaBuilder = SiobhanSchemaBuilder.Create();

			schemaBuilder.AddField(string.Empty, typeof(Guid), false, true);

			lock (syncLock)
				fieldCount = Random.Next(MIN_FIELD_COUNT, MAX_FIELD_COUNT);

			if (fieldCount == INVALID_RANDOM_VALUE)
				throw new NinnelException(nameof(INVALID_RANDOM_VALUE));

			for (long fieldIndex = 0; fieldIndex < fieldCount; fieldIndex++)
			{
				string fieldName = string.Format(FIELD_NAME, fieldIndex);

				schemaBuilder.AddField(fieldName, typeof(double), false, false);
			}

			return schemaBuilder.Build();
		}

		protected override void CoreCreate(bool creating)
		{
			// do nothing
			base.CoreCreate(creating);
		}

		protected override IUnknownSolderConfiguration<EmptySpecification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration)
		{
			return new UnknownNinnelConfiguration<EmptySpecification>(untypedUnknownSolderConfiguration);
		}

		protected override void CoreDispose(bool disposing)
		{
			// do nothing
			base.CoreDispose(disposing);
		}

		protected override INinnelStream CoreInject(NinnelStationFrame ninnelStationFrame)
		{
			INinnelStream stream;
			ISiobhanSchema schema;

			IEnumerable<ISiobhanPayload> payloads;

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

			payloads = GetRandomPayloads(schema);

			if ((object)payloads == null)
				throw new NinnelException(nameof(payloads));

			var records = payloads.Select(p =>
											ninnelStationFrame.NinnelContext
												.CreateProduct(schema, p, SiobhanTopic.None, SiobhanPartition.None,
													SiobhanOffset.Default, SiobhanClock.GetNowAt()))
				.ToLifecycleEnumerable();

			stream = ninnelStationFrame.NinnelContext.CreateStream(records);

			return stream;
		}

		protected override void CorePostExecute(NinnelStationFrame ninnelStationFrame)
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
		}

		protected override void CorePreExecute(NinnelStationFrame ninnelStationFrame)
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
		}

		#endregion
	}
}