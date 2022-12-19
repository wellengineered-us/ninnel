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
	public partial class ConsoleInletStation
		: NinnelInletStation<EmptySpecification>
	{
		#region Methods/Operators
		
		private async static ValueTask<ISiobhanSchema> GetConsoleSchemaAsync()
		{
			long fieldCount;
			SiobhanSchemaBuilder schemaBuilder;
			
			string line;
			string[] fieldNames;

			schemaBuilder = SiobhanSchemaBuilder.Create();
			
			await TextWriter.WriteLineAsync("Enter list of schema field names separated by pipe character: ");
			line = TextReader.ReadLine();
			
			if (!string.IsNullOrEmpty(line))
			{
				fieldNames = line.Split('|');

				if ((object)fieldNames == null || fieldNames.Length <= 0)
				{
					await TextWriter.WriteLineAsync("List of schema field names was invalid; using default (blank).");
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

					await TextWriter.WriteLineAsync(string.Format("Building KEY schema: '{0}'", string.Join(" | ", fieldNames)));
				}
			}

			return schemaBuilder.Build();
		}

		private async IAsyncEnumerable<ISiobhanPayload> GetYieldViaConsoleAsync(ISiobhanSchema schema, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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
				line = await TextReader.ReadLineAsync();

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

			payloads = this.GetYieldViaConsoleAsync(schema, cancellationToken);

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

			schema = await GetConsoleSchemaAsync();

			if ((object)schema == null)
				throw new NinnelException(nameof(schema));

			localState.Add(Constants.ContextComponentScopedSchema, schema);
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