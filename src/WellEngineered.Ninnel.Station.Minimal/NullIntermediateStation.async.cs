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
using WellEngineered.Ninnel.Middleware;

namespace WellEngineered.Ninnel.Station.Minimal
{
	public partial class NullIntermediateStation
		: NinnelIntermediateStation<EmptySpecification>
	{
		#region Methods/Operators

		protected async override ValueTask<IAsyncNinnelStream> CoreProcessAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream asyncNinnelStream, AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> next, CancellationToken cancellationToken = default)
		{
			var retval = (object)next != null ? await next(ninnelStationFrame, asyncNinnelStream, cancellationToken) : asyncNinnelStream;
			return retval;
		}

		#endregion
	}
}
#endif