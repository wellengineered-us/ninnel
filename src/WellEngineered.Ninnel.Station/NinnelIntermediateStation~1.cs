/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Middleware;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Station
{
	public abstract partial class NinnelIntermediateStation<TNinnelSpecification>
		: NinnelStation<TNinnelSpecification>,
			INinnelIntermediateStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Constructors/Destructors

		protected NinnelIntermediateStation()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract INinnelStreamDuality CoreProcess(NinnelStationFrame ninnelStationFrame, INinnelStreamDuality ninnelDualStream, NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStreamDuality> next);

		public INinnelStreamDuality Process(NinnelStationFrame ninnelStationFrame, INinnelStreamDuality ninnelDualStream, NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStreamDuality> next)
		{
			if ((object)ninnelStationFrame == null)
				throw new ArgumentNullException(nameof(ninnelStationFrame));

			if ((object)ninnelDualStream == null)
				throw new ArgumentNullException(nameof(ninnelDualStream));

			//if ((object)next == null)
			//throw new ArgumentNullException(nameof(next));

			try
			{
				return this.CoreProcess(ninnelStationFrame, ninnelDualStream, next);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The intermediate station failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}