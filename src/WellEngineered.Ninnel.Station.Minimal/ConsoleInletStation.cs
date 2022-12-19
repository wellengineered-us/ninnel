/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
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
	public partial class ConsoleInletStation
		: NinnelInletStation<EmptySpecification>
	{
		#region Constructors/Destructors

		public ConsoleInletStation()
		{
		}

		#endregion

		#region Fields/Constants

		private static readonly TextReader textReader = Console.In;
		private static readonly TextWriter textWriter = Console.Out;

		#endregion

		#region Properties/Indexers/Events

		private static TextReader TextReader
		{
			get
			{
				return textReader;
			}
		}

		private static TextWriter TextWriter
		{
			get
			{
				return textWriter;
			}
		}

		#endregion

		#region Methods/Operators

		private IEnumerable<ISiobhanPayload> GetYieldViaConsole(ISiobhanSchema schema)
		{
			ISiobhanPayload payload;
			ISiobhanField[] fields;

			long recordIndex;
			string line;
			string[] fieldValues;

			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			fields = schema.Fields.Values.ToArray();

			recordIndex = 0;
			while (fields.Length > 0)
			{
				line = TextReader.ReadLine();

				if (string.IsNullOrEmpty(line))
					yield break;

				fieldValues = line.Split('|');

				payload = new SiobhanPayload();

				for (long fieldIndex = 0; fieldIndex < Math.Min(fieldValues.Length, fields.Length); fieldIndex++)
					payload.Add(fields[fieldIndex].FieldName, fieldValues[fieldIndex]);

				recordIndex++;

				yield return payload;
			}
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

			payloads = this.GetYieldViaConsole(schema);

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

			schema = GetConsoleSchema();

			if ((object)schema == null)
				throw new NinnelException(nameof(schema));

			localState.Add(Constants.ContextComponentScopedSchema, schema);
		}

		private static ISiobhanSchema GetConsoleSchema()
		{
			long fieldCount;
			SiobhanSchemaBuilder schemaBuilder;
			
			string line;
			string[] fieldNames;

			schemaBuilder = SiobhanSchemaBuilder.Create();
			
			TextWriter.WriteLine("Enter list of schema field names separated by pipe character: ");
			line = TextReader.ReadLine();
			
			if (!string.IsNullOrEmpty(line))
			{
				fieldNames = line.Split('|');

				if ((object)fieldNames == null || fieldNames.Length <= 0)
				{
					TextWriter.WriteLine("List of schema field names was invalid; using default (blank).");
					schemaBuilder.AddField(string.Empty, typeof(string), false, true);
				}
				else
				{
					for (long fieldIndex = 0; fieldIndex < fieldNames.Length; fieldIndex++)
					{
						string fieldName;

						fieldName = fieldNames[fieldIndex];

						if (string.IsNullOrWhiteSpace(fieldName))
							continue;

						schemaBuilder.AddField(fieldName, typeof(string), false, true);
					}

					TextWriter.WriteLine("Building KEY schema: '{0}'", string.Join(" | ", fieldNames));
				}
			}

			return schemaBuilder.Build();
		}

		#endregion
	}
}