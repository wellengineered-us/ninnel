/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Middleware;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Ninnel.Station;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public class DefaultIntermediateStation
		: NinnelIntermediateStation<EmptySpecification>
	{
		#region Constructors/Destructors

		public DefaultIntermediateStation()
		{
		}

		#endregion

		#region Methods/Operators

		protected override IUnknownSolderConfiguration<EmptySpecification> CoreCreateGenericTypedUnknownConfiguration()
		{
			return new UnknownNinnelConfiguration<EmptySpecification>(this.Configuration);
		}

		protected override void CorePostExecute(NinnelStationFrame ninnelStationFrame)
		{
		}

		protected override void CorePreExecute(NinnelStationFrame ninnelStationFrame)
		{
		}

		protected override ValueTask CorePreExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override INinnelStreamDuality CoreProcess(NinnelStationFrame ninnelStationFrame, INinnelStreamDuality ninnelDualStream, NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStreamDuality> next)
		{
			return null;
		}

		protected override ValueTask<INinnelStreamDuality> CoreProcessAsync(NinnelStationFrame ninnelStationFrame, INinnelStreamDuality ninnelDualStream, AsyncNinnelMiddlewareDelegate<NinnelStationFrame, INinnelStreamDuality> next, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override ValueTask CorPostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			return default;
		}

		#endregion
	}
}