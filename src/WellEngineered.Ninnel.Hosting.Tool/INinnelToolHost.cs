/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public interface INinnelToolHost : INinnelHost<ToolHostConfiguration>
	{
		#region Methods/Operators

		void Host(IDictionary<string, IList<object>> arguments,
			IDictionary<string, IList<object>> properties);

		ValueTask HostAsync(IDictionary<string, IList<object>> arguments,
			IDictionary<string, IList<object>> properties, CancellationToken cancellationToken = default);

		#endregion
	}
}