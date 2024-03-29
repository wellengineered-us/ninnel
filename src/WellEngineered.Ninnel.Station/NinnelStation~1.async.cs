/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public abstract partial class NinnelStation<TNinnelSpecification>
		: NinnelComponent<IUnknownNinnelConfiguration<TNinnelSpecification>, TNinnelSpecification>,
			INinnelStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Methods/Operators

		protected abstract ValueTask CorePreExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default);

		protected abstract ValueTask CorPostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default);

		public ValueTask PostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelStationFrame == null)
				throw new ArgumentNullException(nameof(ninnelStationFrame));

			try
			{
				return this.CorPostExecuteAsync(ninnelStationFrame, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The station failed (see inner exception)."), ex);
			}
		}

		public ValueTask PreExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelStationFrame == null)
				throw new ArgumentNullException(nameof(ninnelStationFrame));

			try
			{
				return this.CorePreExecuteAsync(ninnelStationFrame, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The station failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}
#endif