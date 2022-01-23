/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Context
{
	public partial interface INinnelContextFactory
		: INinnelComponent0
	{
		#region Methods/Operators

		ValueTask<INinnelContext> CloneContextAsync(INinnelContext ninnelContext, CancellationToken cancellationToken = default);

		ValueTask<INinnelContext> CreateContextAsync(CancellationToken cancellationToken = default);

		#endregion
	}
}