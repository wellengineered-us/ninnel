/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Middleware;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Station.Minimal
{
	public partial class NullIntermediateStation
		: NinnelIntermediateStation<EmptySpecification>
	{
		#region Constructors/Destructors

		public NullIntermediateStation()
		{
		}

		#endregion

		#region Methods/Operators

		protected override IUnknownSolderConfiguration<EmptySpecification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration)
		{
			return new UnknownNinnelConfiguration<EmptySpecification>(untypedUnknownSolderConfiguration);
		}

		protected override INinnelStream CoreProcess(NinnelStationFrame ninnelStationFrame, INinnelStream ninnelStream, NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> next)
		{
			var retval = (object)next != null ? next(ninnelStationFrame, ninnelStream) : ninnelStream;
			return retval;
		}

		#endregion
	}
}