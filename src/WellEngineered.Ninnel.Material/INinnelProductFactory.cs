/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Siobhan.Model;

namespace WellEngineered.Ninnel.Material
{
	public partial interface INinnelProductFactory
		: INinnelComponent0
	{
		#region Methods/Operators

		INinnelProduct CreateEmptyProduct();

		INinnelProduct CreateProduct(ISiobhanSchema schema, ISiobhanPayload payload, ISiobhanTopic topic,
			ISiobhanPartition partition, ISiobhanOffset offset, ISiobhanClock clock);

		#endregion
	}
}