/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Siobhan.Model;
using WellEngineered.Siobhan.Primitives;
using WellEngineered.Siobhan.Textual;
using WellEngineered.Solder.Configuration;

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
		#region Constructors/Destructors

		public TextualInletStation()
		{
		}

		#endregion

		#region Fields/Constants

		private TTextualReader textualReader;

		#endregion

		#region Properties/Indexers/Events

		protected TTextualReader TextualReader
		{
			get
			{
				return this.textualReader;
			}
			private set
			{
				this.textualReader = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreCreate(bool creating)
		{
			// do nothing
			base.CoreCreate(creating);
		}

		protected override IUnknownSolderConfiguration<TTextualSpecification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration)
		{
			return new UnknownNinnelConfiguration<TTextualSpecification>(untypedUnknownSolderConfiguration);
		}

		protected abstract TTextualReader CoreCreateTextualReader(StreamReader streamReader, TTextualSpec textualSpec);

		protected override void CoreDispose(bool disposing)
		{
			if (disposing)
			{
				if ((object)this.TextualReader != null)
				{
					this.TextualReader.Dispose();
					this.TextualReader = null;
				}
			}

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

			payloads = this.GetTextualPayloads(schema);

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

			if ((object)this.TextualReader != null)
			{
				this.TextualReader.Dispose();
				this.TextualReader = null;
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

			schema = this.GetTextualSchema();

			if ((object)schema == null)
				throw new NinnelException(nameof(schema));

			localState.Add(Constants.ContextComponentScopedSchema, schema);
		}

		private IEnumerable<ISiobhanPayload> GetTextualPayloads(ISiobhanSchema schema)
		{
			TTextualSpecification specification;
			TTextualSpec spec;

			IEnumerable<ISiobhanPayload> payloads;

			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			specification = this.Configuration.Specification;

			if ((object)specification == null)
				throw new NinnelException(nameof(specification));

			spec = specification.TextualConfiguration.MapToSpec();

			if ((object)spec == null)
				throw new NinnelException(nameof(spec));

			payloads = this.TextualReader.ReadRecords();

			if ((object)payloads == null)
				throw new NinnelException(nameof(payloads));

			return payloads;
		}

		private ISiobhanSchema GetTextualSchema()
		{
			SiobhanSchemaBuilder schemaBuilder;

			TTextualSpecification specification;
			TTextualSpec spec;
			IEnumerable<TTextualFieldSpec> headers;

			specification = this.Configuration.Specification;
			spec = specification.TextualConfiguration.MapToSpec();

			if ((object)spec == null)
				throw new NinnelException(nameof(spec));

			this.TextualReader = this.CoreCreateTextualReader(new StreamReader(File.Open(specification.TextualFilePath, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.GetEncoding(specification.TextualConfiguration.ContentEncoding)), spec);

			headers = this.TextualReader.ReadHeaderFields();

			if ((object)headers == null)
				throw new NinnelException(nameof(headers));

			schemaBuilder = SiobhanSchemaBuilder.Create();

			headers.ForceEnumeration();

			foreach (TTextualFieldSpec header in spec.HeaderSpecs)
				schemaBuilder.AddField(header.FieldTitle, header.FieldType.ToClrType(), header.IsFieldRequired, header.IsFieldIdentity);

			return schemaBuilder.Build();
		}

		#endregion
	}
}