/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Hosting
{
	public partial interface INinnelHostFactory
		: INinnelComponent0
	{
		#region Methods/Operators

		ValueTask<INinnelHost> CreateHostAsync(CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif