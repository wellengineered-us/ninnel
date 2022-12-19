/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Siobhan.Model;

namespace WellEngineered.Ninnel.Material
{
	public interface INinnelProduct
		: INinnelComponent0
	{
		#region Properties/Indexers/Events

		ISiobhanClock Clock
		{
			get;
		}

		ISiobhanOffset Offset
		{
			get;
		}

		ISiobhanPartition Partition
		{
			get;
		}

		ISiobhanPayload Payload
		{
			get;
		}

		ISiobhanSchema Schema
		{
			get;
		}

		ISiobhanTopic Topic
		{
			get;
		}

		long ClockRelativeIndex
		{
			set;
		}

		#endregion
	}
}