/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Middleware;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Station
{
	public abstract partial class NinnelIntermediateStation<TNinnelSpecification>
		: NinnelStation<TNinnelSpecification>,
			INinnelIntermediateStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Methods/Operators
		
		protected sealed override ValueTask CorePreExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			throw new NinnelException(string.Format("Pre execution semantics are not supported for this component type."));
		}

		protected sealed override ValueTask CorPostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			throw new NinnelException(string.Format("Post execution semantics are not supported for this component type."));
		}

		protected abstract ValueTask<IAsyncNinnelStream> CoreProcessAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream asyncNinnelStream, AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> next, CancellationToken cancellationToken = default);

		public ValueTask<IAsyncNinnelStream> ProcessAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream asyncNinnelStream, AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> next, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelStationFrame == null)
				throw new ArgumentNullException(nameof(ninnelStationFrame));

			//if ((object)asyncNinnelStream == null)
			//throw new ArgumentNullException(nameof(asyncNinnelStream));

			//if ((object)next == null)
			//throw new ArgumentNullException(nameof(next));

			try
			{
				return this.CoreProcessAsync(ninnelStationFrame, asyncNinnelStream, next, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The intermediate station failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}
#endif