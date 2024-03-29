/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public abstract partial class NinnelOutletStation<TNinnelSpecification>
		: NinnelStation<TNinnelSpecification>,
			INinnelOutletStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Constructors/Destructors

		protected NinnelOutletStation()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract void CoreDeliver(NinnelStationFrame ninnelStationFrame, INinnelStream ninnelStream);

		public void Deliver(NinnelStationFrame ninnelStationFrame, INinnelStream ninnelStream)
		{
			if ((object)ninnelStationFrame == null)
				throw new ArgumentNullException(nameof(ninnelStationFrame));

			//if ((object)ninnelStream == null)
			//throw new ArgumentNullException(nameof(ninnelStream));

			try
			{
				this.CoreDeliver(ninnelStationFrame, ninnelStream);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The outlet station failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}