/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using WellEngineered.Siobhan.Primitives;

namespace WellEngineered.Ninnel.Material
{
	public abstract class AsyncNinnelStream
		: AsyncForEachLifecycleYieldStateMachine<INinnelProduct, INinnelProduct>,
			IAsyncNinnelStream
	{
		#region Constructors/Destructors

		protected AsyncNinnelStream(IAsyncLifecycleEnumerable<INinnelProduct> baseAsyncEnumerable)
			: base(baseAsyncEnumerable, async (ix, p) => await Task.FromResult(p))
		{
		}

		#endregion
	}
}