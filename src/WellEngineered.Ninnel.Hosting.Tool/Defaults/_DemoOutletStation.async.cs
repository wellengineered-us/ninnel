/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Ninnel.Station;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Hosting.Tool.Defaults
{
	public partial class _DemoOutletStation : NinnelOutletStation<_DemoSpecification>
	{
		protected async override ValueTask CorePreExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			await Console.Out.WriteLineAsync(string.Format("DEMO: outlet station PRE-EXECUTE(async)"));
		}

		protected async override ValueTask CorPostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			await Console.Out.WriteLineAsync(string.Format("DEMO: outlet station POST-EXECUTE(async)"));
		}

		protected async override ValueTask CoreDeliverAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream ninnelStream, CancellationToken cancellationToken = default)
		{
			await Console.Out.WriteLineAsync(string.Format("DEMO: outlet station DELIVER(async)"));
		}
	}
}
#endif