/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Station.Minimal
{
	public partial class NullOutletStation
		: NinnelOutletStation<EmptySpecification>
	{
		#region Constructors/Destructors

		public NullOutletStation()
		{
		}

		#endregion

		#region Methods/Operators

		protected override IUnknownSolderConfiguration<EmptySpecification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration)
		{
			return new UnknownNinnelConfiguration<EmptySpecification>(untypedUnknownSolderConfiguration);
		}

		protected override void CoreDeliver(NinnelStationFrame ninnelStationFrame, INinnelStream ninnelStream)
		{
		}

		protected override void CorePostExecute(NinnelStationFrame ninnelStationFrame)
		{
		}

		protected override void CorePreExecute(NinnelStationFrame ninnelStationFrame)
		{
		}

		#endregion
	}
}