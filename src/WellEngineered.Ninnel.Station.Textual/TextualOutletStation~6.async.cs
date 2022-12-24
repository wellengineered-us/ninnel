/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN

using System;
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
		#region Methods/Operators

		protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = new CancellationToken())
		{
			// do nothing
			return base.CoreCreateAsync(creating, cancellationToken);
		}

		protected async override ValueTask CoreDeliverAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream ninnelStream, CancellationToken cancellationToken = default)
		{
			IAsyncLifecycleEnumerable<ISiobhanPayload> payloads;

			if ((object)ninnelStream == null)
				throw new ArgumentNullException(nameof(ninnelStream));

			if ((object)ninnelStationFrame.NinnelContext == null)
				throw new NinnelException(nameof(ninnelStationFrame.NinnelContext));

			if ((object)ninnelStationFrame.RecordConfiguration == null)
				throw new NinnelException(nameof(ninnelStationFrame.RecordConfiguration));

			payloads = ninnelStream.Select(p => p.Payload).ToAsyncLifecycleEnumerable();

			await this.TextualWriter.WriteRecordsAsync(payloads, cancellationToken);
		}

		protected async override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			if (disposing)
			{
				if ((object)this.TextualWriter != null)
				{
					await this.TextualWriter.DisposeAsync(cancellationToken);
					this.TextualWriter = null;
				}
			}

			await base.CoreDisposeAsync(disposing, cancellationToken);
		}

		protected override ValueTask CorePreExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected async override ValueTask CorPostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelStationFrame.NinnelContext == null)
				throw new NinnelException(nameof(ninnelStationFrame.NinnelContext));

			if ((object)ninnelStationFrame.RecordConfiguration == null)
				throw new NinnelException(nameof(ninnelStationFrame.RecordConfiguration));

			if ((object)this.TextualWriter != null)
			{
				await this.TextualWriter.FlushAsync(cancellationToken);
				await this.TextualWriter.DisposeAsync(cancellationToken);
				this.TextualWriter = null;
			}
		}

		#endregion
	}
}
#endif