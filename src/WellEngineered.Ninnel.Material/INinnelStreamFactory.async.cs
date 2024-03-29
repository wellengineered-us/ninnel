/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;

using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Siobhan.Primitives;

namespace WellEngineered.Ninnel.Material
{
	public partial interface INinnelStreamFactory
		: INinnelComponent0
	{
		#region Methods/Operators

		IAsyncNinnelStream CreateEmptyStreamAsync(CancellationToken cancellationToken = default);

		IAsyncNinnelStream CreateStreamAsync(IAsyncLifecycleEnumerable<INinnelProduct> ninnelRecords, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif