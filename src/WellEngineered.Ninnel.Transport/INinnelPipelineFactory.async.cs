/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Transport
{
	public partial interface INinnelPipelineFactory
		: INinnelComponent0
	{
		#region Methods/Operators

		ValueTask<INinnelPipeline> CreatePipelineAsync(Type ninnelPipelineType, CancellationToken cancellationToken = default);

		#endregion
	}
}