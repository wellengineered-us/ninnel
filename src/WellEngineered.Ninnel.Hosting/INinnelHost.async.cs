/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Hosting
{
	public partial interface INinnelHost
		: INinnelComponent0
	{
		#region Methods/Operators

		ValueTask HostAsync(CancellationToken cancellationToken = default);

		#endregion
	}
}