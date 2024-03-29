/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public abstract partial class NinnelOutletStation<TNinnelSpecification>
		: NinnelStation<TNinnelSpecification>,
			INinnelOutletStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Methods/Operators

		protected abstract ValueTask CoreDeliverAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream ninnelStream, CancellationToken cancellationToken = default);

		public ValueTask DeliverAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream ninnelStream, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelStationFrame == null)
				throw new ArgumentNullException(nameof(ninnelStationFrame));

			//if ((object)ninnelStream == null)
			//throw new ArgumentNullException(nameof(ninnelStream));

			try
			{
				return this.CoreDeliverAsync(ninnelStationFrame, ninnelStream, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The outlet station failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}
#endif