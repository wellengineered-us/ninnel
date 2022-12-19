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
using WellEngineered.Siobhan.Primitives;

namespace WellEngineered.Ninnel.Station.Minimal
{
	public partial class NullOutletStation
		: NinnelOutletStation<EmptySpecification>
	{
		#region Methods/Operators

		protected override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = new CancellationToken())
		{
			// do nothing
			return base.CoreCreateAsync(creating, cancellationToken);
		}

		protected async override ValueTask CoreDeliverAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream ninnelStream, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelStream == null)
				throw new ArgumentNullException(nameof(ninnelStream));

			if ((object)ninnelStationFrame.NinnelContext == null)
				throw new NinnelException(nameof(ninnelStationFrame.NinnelContext));

			if ((object)ninnelStationFrame.RecordConfiguration == null)
				throw new NinnelException(nameof(ninnelStationFrame.RecordConfiguration));

			await ninnelStream.ForceAsyncEnumeration(cancellationToken); // force execution
		}

		protected override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = new CancellationToken())
		{
			// do nothing
			return base.CoreDisposeAsync(disposing, cancellationToken);
		}

		protected override ValueTask CorePreExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		protected override ValueTask CorPostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			// do nothing
			return default;
		}

		#endregion
	}
}
#endif