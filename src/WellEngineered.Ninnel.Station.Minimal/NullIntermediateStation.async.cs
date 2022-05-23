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
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Station.Minimal
{
	public partial class NullIntermediateStation
		: NinnelIntermediateStation<EmptySpecification>
	{
		#region Methods/Operators

		protected override IUnknownSolderConfiguration<EmptySpecification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration)
		{
			return new UnknownNinnelConfiguration<EmptySpecification>(untypedUnknownSolderConfiguration);
		}

		protected override ValueTask<IAsyncNinnelStream> CoreProcessAsync(NinnelStationFrame ninnelStationFrame, IAsyncNinnelStream asyncNinnelStream, AsyncNinnelMiddlewareDelegate<NinnelStationFrame, IAsyncNinnelStream> next, CancellationToken cancellationToken = default)
		{
			return default;
		}

		#endregion
	}
}
#endif