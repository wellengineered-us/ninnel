/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public partial interface INinnelToolHost
		: INinnelHost<ToolHostConfiguration>
	{
		#region Methods/Operators

		ValueTask HostAsync(IDictionary<string, IList<object>> arguments,
			IDictionary<string, IList<object>> properties, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif