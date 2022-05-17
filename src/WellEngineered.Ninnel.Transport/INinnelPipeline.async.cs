/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Context;

namespace WellEngineered.Ninnel.Transport
{
	public partial interface INinnelPipeline
		: INinnelTransport<PipelineConfiguration>,
			INinnelContextFactory
	{
		#region Methods/Operators

		ValueTask<long> ExecuteAsync(INinnelContext ninnelContext, CancellationToken cancellationToken = default);

		#endregion
	}
}