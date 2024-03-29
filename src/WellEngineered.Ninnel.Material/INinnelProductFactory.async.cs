/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Siobhan.Model;

namespace WellEngineered.Ninnel.Material
{
	public partial interface INinnelProductFactory
		: INinnelComponent0
	{
		#region Methods/Operators

		ValueTask<INinnelProduct> CreateEmptyProductAsync(CancellationToken cancellationToken = default);

		ValueTask<INinnelProduct> CreateProductAsync(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic,
			ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif