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
	public abstract partial class NinnelInletStation<TNinnelSpecification>
		: NinnelStation<TNinnelSpecification>,
			INinnelInletStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Methods/Operators

		protected abstract ValueTask<IAsyncNinnelStream> CoreInjectAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default);

		public ValueTask<IAsyncNinnelStream> InjectAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelStationFrame == null)
				throw new ArgumentNullException(nameof(ninnelStationFrame));

			try
			{
				return this.CoreInjectAsync(ninnelStationFrame, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The inlet station failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}
#endif