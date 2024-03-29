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
	public abstract partial class NinnelInletStation<TNinnelSpecification>
		: NinnelStation<TNinnelSpecification>,
			INinnelInletStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Constructors/Destructors

		protected NinnelInletStation()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract INinnelStream CoreInject(NinnelStationFrame ninnelStationFrame);

		public INinnelStream Inject(NinnelStationFrame ninnelStationFrame)
		{
			if ((object)ninnelStationFrame == null)
				throw new ArgumentNullException(nameof(ninnelStationFrame));

			try
			{
				return this.CoreInject(ninnelStationFrame);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The inlet station failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}