/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Transport;

namespace WellEngineered.Ninnel.Hosting
{
	public partial interface INinnelHost<THostConfiguration>
		: INinnelHost,
			INinnelComponent<THostConfiguration>,
			INinnelPipelineFactory
		where THostConfiguration : HostConfiguration
	{
		#region Methods/Operators

		ValueTask CancelAsync(CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif