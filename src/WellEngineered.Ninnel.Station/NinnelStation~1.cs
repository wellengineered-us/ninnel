/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public abstract partial class NinnelStation<TNinnelSpecification>
		: NinnelComponent<IUnknownNinnelConfiguration<TNinnelSpecification>, TNinnelSpecification>,
			INinnelStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Constructors/Destructors

		protected NinnelStation()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract void CorePostExecute(NinnelStationFrame ninnelStationFrame);

		protected abstract void CorePreExecute(NinnelStationFrame ninnelStationFrame);

		public void PostExecute(NinnelStationFrame ninnelStationFrame)
		{
			if ((object)ninnelStationFrame == null)
				throw new ArgumentNullException(nameof(ninnelStationFrame));

			try
			{
				this.CorePostExecute(ninnelStationFrame);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The station failed (see inner exception)."), ex);
			}
		}

		public void PreExecute(NinnelStationFrame ninnelStationFrame)
		{
			if ((object)ninnelStationFrame == null)
				throw new ArgumentNullException(nameof(ninnelStationFrame));

			try
			{
				this.CorePreExecute(ninnelStationFrame);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The station failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}