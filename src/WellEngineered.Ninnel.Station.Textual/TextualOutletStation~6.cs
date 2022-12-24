/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
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
	public abstract partial class TextualOutletStation<
			TTextualFieldConfiguration,
			TTextualConfiguration,
			TTextualFieldSpec,
			TTextualSpec,
			TTextualSpecification,
			TTextualWriter>
		: NinnelOutletStation<TTextualSpecification>
		where TTextualFieldConfiguration : TextualFieldConfiguration
		where TTextualConfiguration : TextualConfiguration<TTextualFieldConfiguration, TTextualFieldSpec, TTextualSpec>
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
		where TTextualSpecification : TextualSpecification<TTextualFieldConfiguration, TTextualConfiguration, TTextualFieldSpec, TTextualSpec>, new()
		where TTextualWriter : TextualWriter<TTextualFieldSpec, TTextualSpec>
	{
		#region Constructors/Destructors

		public TextualOutletStation()
		{
		}

		#endregion

		#region Fields/Constants

		private TTextualWriter textualWriter;

		#endregion

		#region Properties/Indexers/Events

		protected TTextualWriter TextualWriter
		{
			get
			{
				return this.textualWriter;
			}
			private set
			{
				this.textualWriter = value;
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

		protected abstract TTextualWriter CoreCreateTextualWriter(StreamWriter streamWriter, TTextualSpec textualSpec);

		protected override void CoreDeliver(NinnelStationFrame ninnelStationFrame, INinnelStream ninnelStream)
		{
			ILifecycleEnumerable<ISiobhanPayload> payloads;

			if ((object)ninnelStream == null)
				throw new ArgumentNullException(nameof(ninnelStream));

			if ((object)ninnelStationFrame.NinnelContext == null)
				throw new NinnelException(nameof(ninnelStationFrame.NinnelContext));

			if ((object)ninnelStationFrame.RecordConfiguration == null)
				throw new NinnelException(nameof(ninnelStationFrame.RecordConfiguration));

			payloads = ninnelStream.Select(p => p.Payload).ToLifecycleEnumerable();

			this.TextualWriter.WriteRecords(payloads);
		}

		protected override void CoreDispose(bool disposing)
		{
			if (disposing)
			{
				if ((object)this.TextualWriter != null)
				{
					this.TextualWriter.Dispose();
					this.TextualWriter = null;
				}
			}

			base.CoreDispose(disposing);
		}

		protected override void CorePostExecute(NinnelStationFrame ninnelStationFrame)
		{
			if ((object)ninnelStationFrame.NinnelContext == null)
				throw new NinnelException(nameof(ninnelStationFrame.NinnelContext));

			if ((object)ninnelStationFrame.RecordConfiguration == null)
				throw new NinnelException(nameof(ninnelStationFrame.RecordConfiguration));

			if ((object)this.TextualWriter != null)
			{
				this.TextualWriter.Flush();
				this.TextualWriter.Dispose();
				this.TextualWriter = null;
			}
		}

		protected override void CorePreExecute(NinnelStationFrame ninnelStationFrame)
		{
			TTextualSpecification specification;
			TTextualSpec spec;
			string filePath;

			if ((object)ninnelStationFrame.NinnelContext == null)
				throw new NinnelException(nameof(ninnelStationFrame.NinnelContext));

			if ((object)ninnelStationFrame.RecordConfiguration == null)
				throw new NinnelException(nameof(ninnelStationFrame.RecordConfiguration));

			specification = this.Configuration.Specification;

			if ((object)specification == null)
				throw new NinnelException(nameof(specification));

			spec = specification.TextualConfiguration.MapToSpec();

			if ((object)spec == null)
				throw new NinnelException(nameof(spec));

			if (Directory.Exists(specification.TextualFilePath))
				filePath = Path.Combine(specification.TextualFilePath, Path.GetRandomFileName());
			else
				filePath = specification.TextualFilePath;

			this.TextualWriter = this.CoreCreateTextualWriter(new StreamWriter(File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None), Encoding.GetEncoding(specification.TextualConfiguration.ContentEncoding)), spec);
		}

		#endregion
	}
}