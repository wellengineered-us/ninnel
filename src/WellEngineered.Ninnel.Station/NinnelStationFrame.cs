/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Context;

namespace WellEngineered.Ninnel.Station
{
	public struct NinnelStationFrame
	{
		#region Constructors/Destructors

		public NinnelStationFrame(INinnelContext ninnelContext, RecordConfiguration recordConfiguration)
		{
			if ((object)ninnelContext == null)
				throw new ArgumentNullException(nameof(ninnelContext));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			this.ninnelContext = ninnelContext;
			this.recordConfiguration = recordConfiguration;
		}

		#endregion

		#region Fields/Constants

		private readonly INinnelContext ninnelContext;
		private readonly RecordConfiguration recordConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public INinnelContext NinnelContext
		{
			get
			{
				return this.ninnelContext;
			}
		}

		public RecordConfiguration RecordConfiguration
		{
			get
			{
				return this.recordConfiguration;
			}
		}

		#endregion
	}
}