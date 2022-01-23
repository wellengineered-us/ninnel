/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Middleware;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public abstract partial class NinnelIntermediateStation<TNinnelSpecification>
		: NinnelStation<TNinnelSpecification>,
			INinnelIntermediateStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Methods/Operators

		protected abstract ValueTask<INinnelStreamDuality> CoreProcessAsync(NinnelStationFrame ninnelStationFrame, INinnelStreamDuality ninnelDualStream, AsyncNinnelMiddlewareDelegate<NinnelStationFrame, INinnelStreamDuality> next, CancellationToken cancellationToken = default);

		public ValueTask<INinnelStreamDuality> ProcessAsync(NinnelStationFrame ninnelStationFrame, INinnelStreamDuality ninnelDualStream, AsyncNinnelMiddlewareDelegate<NinnelStationFrame, INinnelStreamDuality> next, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelStationFrame == null)
				throw new ArgumentNullException(nameof(ninnelStationFrame));

			if ((object)ninnelDualStream == null)
				throw new ArgumentNullException(nameof(ninnelDualStream));

			//if ((object)next == null)
			//throw new ArgumentNullException(nameof(next));

			try
			{
				return this.CoreProcessAsync(ninnelStationFrame, ninnelDualStream, next, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The intermediate station failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}