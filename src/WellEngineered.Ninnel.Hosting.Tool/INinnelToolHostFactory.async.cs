/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public partial interface INinnelToolHostFactory
		: INinnelHostFactory
	{
		#region Methods/Operators

		ValueTask<INinnelToolHost> CreateToolHostAsync(ToolHostConfiguration toolHostConfiguration, CancellationToken cancellationToken = default);

		ValueTask<INinnelToolHost> CreateToolHostAsync(Uri toolHostConfigFileUri, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif