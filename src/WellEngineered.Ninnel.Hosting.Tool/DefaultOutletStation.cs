/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Ninnel.Station;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public class DefaultOutletStation
		: NinnelOutletStation<EmptySpecification>
	{
		#region Constructors/Destructors

		public DefaultOutletStation()
		{
		}

		#endregion

		#region Methods/Operators

		protected override IUnknownSolderConfiguration<EmptySpecification> CoreCreateGenericTypedUnknownConfiguration()
		{
			return new UnknownNinnelConfiguration<EmptySpecification>(this.Configuration);
		}

		protected override void CoreDeliver(NinnelStationFrame ninnelStationFrame, INinnelStream ninnelStream)
		{
		}

		protected override ValueTask CoreDeliverAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream ninnelStream, CancellationToken cancellationToken = default)
		{
			return default;
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

		protected override ValueTask CorPostExecuteAsync(NinnelStationFrame ninnelStationFrame, CancellationToken cancellationToken = default)
		{
			return default;
		}

		#endregion
	}
}