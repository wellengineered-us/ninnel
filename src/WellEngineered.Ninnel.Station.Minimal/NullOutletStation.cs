/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Siobhan.Primitives;
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

		protected override void CoreCreate(bool creating)
		{
			// do nothing
			base.CoreCreate(creating);
		}

		protected override IUnknownSolderConfiguration<EmptySpecification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration)
		{
			return new UnknownNinnelConfiguration<EmptySpecification>(untypedUnknownSolderConfiguration);
		}

		protected override void CoreDeliver(NinnelStationFrame ninnelStationFrame, INinnelStream ninnelStream)
		{
			if ((object)ninnelStream == null)
				throw new ArgumentNullException(nameof(ninnelStream));

			if ((object)ninnelStationFrame.NinnelContext == null)
				throw new NinnelException(nameof(ninnelStationFrame.NinnelContext));

			if ((object)ninnelStationFrame.RecordConfiguration == null)
				throw new NinnelException(nameof(ninnelStationFrame.RecordConfiguration));

			ninnelStream.ForceEnumeration(); // force execution
		}

		protected override void CoreDispose(bool disposing)
		{
			// do nothing
			base.CoreDispose(disposing);
		}

		protected override void CorePostExecute(NinnelStationFrame ninnelStationFrame)
		{
			// do nothing
		}

		protected override void CorePreExecute(NinnelStationFrame ninnelStationFrame)
		{
			// do nothing
		}

		#endregion
	}
}