/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Middleware;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Ninnel.Station;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Hosting.Tool.Defaults
{
	public partial class _DemoIntermediateStation : NinnelIntermediateStation<_DemoSpecification>
	{
		#region Methods/Operators

		protected async override ValueTask<IAsyncNinnelStream> CoreProcessAsync(NinnelStationFrame data, IAsyncNinnelStream target, AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> next, CancellationToken cancellationToken = default)
		{
			IAsyncNinnelStream newNinnelStream;

			if ((object)data.NinnelContext == null)
				throw new NinnelException(nameof(data.NinnelContext));

			if ((object)data.RecordConfiguration == null)
				throw new NinnelException(nameof(data.RecordConfiguration));

			await Console.Out.WriteLineAsync(string.Format("DEMO: {0} intermediate station BEFORE(async)", this._));

			if ((object)next != null)
				newNinnelStream = await next(data, target, cancellationToken);
			else
				newNinnelStream = target;

			await Console.Out.WriteLineAsync(string.Format("DEMO: {0} intermediate station AFTER(async)", this._));

			return newNinnelStream;
		}

		#endregion
	}
}