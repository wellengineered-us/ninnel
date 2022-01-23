/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public interface INinnelToolHostFactory : INinnelHostFactory
	{
		#region Methods/Operators

		INinnelToolHost CreateToolHost(ToolHostConfiguration toolHostConfiguration);

		INinnelToolHost CreateToolHost(Uri toolHostConfigFileUri);

		ValueTask<INinnelToolHost> CreateToolHostAsync(ToolHostConfiguration toolHostConfiguration, CancellationToken cancellationToken = default);

		ValueTask<INinnelToolHost> CreateToolHostAsync(Uri toolHostConfigFileUri, CancellationToken cancellationToken = default);

		#endregion
	}
}