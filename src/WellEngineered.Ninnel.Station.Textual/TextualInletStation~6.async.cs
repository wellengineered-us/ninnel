/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Siobhan.Model;
using WellEngineered.Siobhan.Primitives;
using WellEngineered.Siobhan.Textual;

namespace WellEngineered.Ninnel.Station.Textual
{
	public abstract partial class TextualInletStation<
			TTextualFieldConfiguration,
			TTextualConfiguration,
			TTextualFieldSpec,
			TTextualSpec,
			TTextualSpecification,
			TTextualReader>
		: NinnelInletStation<TTextualSpecification>
		where TTextualFieldConfiguration : TextualFieldConfiguration
		where TTextualConfiguration : TextualConfiguration<TTextualFieldConfiguration, TTextualFieldSpec, TTextualSpec>
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
		where TTextualSpecification : TextualSpecification<TTextualFieldConfiguration, TTextualConfiguration, TTextualFieldSpec, TTextualSpec>, new()
		where TTextualReader : TextualReader<TTextualFieldSpec, TTextualSpec>
	{
		#region Methods/Operators

		protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = new CancellationToken())
		{
			// do nothing
			return base.CoreCreateAsync(creating, cancellationToken);
		}

		protected async override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = new CancellationToken())
		{
			if (disposing)
			{
				if ((object)this.TextualReader != null)
				{
					await this.TextualReader.DisposeAsync(cancellationToken);
					this.TextualReader = null;
				}
			}

			await base.CoreDisposeAsync(disposing, cancellationToken);
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

			payloads = this.GetTextualPayloadsAsync(schema, cancellationToken);

			if ((object)payloads == null)
				throw new NinnelException(nameof(payloads));

			var records = payloads.Select(p =>
											ninnelStationFrame.NinnelContext
												.CreateProduct(schema, p, SiobhanTopic.None, SiobhanPartition.None,
													SiobhanOffset.Default, SiobhanClock.GetNowAt()))
				.ToAsyncLifecycleEnumerable();

			stream = ninnelStationFrame.NinnelContext.CreateStreamAsync(records);

			await Task.CompletedTask;
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

			schema = await this.GetTextualSchemaAsync(cancellationToken);

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

			if ((object)this.TextualReader != null)
			{
				await this.TextualReader.DisposeAsync(cancellationToken);
				this.TextualReader = null;
			}
		}

		private IAsyncEnumerable<ISiobhanPayload> GetTextualPayloadsAsync(ISiobhanSchema schema, CancellationToken cancellationToken = default)
		{
			TTextualSpecification specification;
			TTextualSpec spec;

			IAsyncEnumerable<ISiobhanPayload> payloads;

			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			specification = this.Configuration.Specification;

			if ((object)specification == null)
				throw new NinnelException(nameof(specification));

			spec = specification.TextualConfiguration.MapToSpec();

			if ((object)spec == null)
				throw new NinnelException(nameof(spec));

			payloads = this.TextualReader.ReadRecordsAsync(cancellationToken);

			if ((object)payloads == null)
				throw new NinnelException(nameof(payloads));

			return payloads;
		}

		private async ValueTask<ISiobhanSchema> GetTextualSchemaAsync(CancellationToken cancellationToken = default)
		{
			SiobhanSchemaBuilder schemaBuilder;

			TTextualSpecification specification;
			TTextualSpec spec;
			IAsyncEnumerable<TTextualFieldSpec> headers;

			specification = this.Configuration.Specification;
			spec = specification.TextualConfiguration.MapToSpec();

			if ((object)spec == null)
				throw new NinnelException(nameof(spec));

			this.TextualReader = this.CoreCreateTextualReader(new StreamReader(File.Open(specification.TextualFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)), spec);

			headers = this.TextualReader.ReadHeaderFieldsAsync(cancellationToken);

			if ((object)headers == null)
				throw new NinnelException(nameof(headers));

			schemaBuilder = SiobhanSchemaBuilder.Create();

			await headers.ForceAsyncEnumeration(cancellationToken);

			foreach (TTextualFieldSpec header in spec.HeaderSpecs)
				schemaBuilder.AddField(header.FieldTitle, header.FieldType.ToClrType(), header.IsFieldRequired, header.IsFieldIdentity);

			return schemaBuilder.Build();
		}

		#endregion
	}
}
#endif