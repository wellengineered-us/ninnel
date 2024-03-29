/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Configuration;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public class ToolHostConfiguration
		: HostConfiguration
	{
		#region Constructors/Destructors

		public ToolHostConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private TimeSpan? dispatchIdleTimeSpan;
		private bool? enableDispatchLoop;
		private TimeSpan? gracefulShutdownTimeSpan;

		#endregion

		#region Properties/Indexers/Events

		public TimeSpan? DispatchIdleTimeSpan
		{
			get
			{
				return this.dispatchIdleTimeSpan;
			}
			set
			{
				this.dispatchIdleTimeSpan = value;
			}
		}

		public bool? EnableDispatchLoop
		{
			get
			{
				return this.enableDispatchLoop;
			}
			set
			{
				this.enableDispatchLoop = value;
			}
		}

		public TimeSpan? GracefulShutdownTimeSpan
		{
			get
			{
				return this.gracefulShutdownTimeSpan;
			}
			set
			{
				this.gracefulShutdownTimeSpan = value;
			}
		}

		#endregion
	}
}