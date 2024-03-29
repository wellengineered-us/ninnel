/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Transport
{
	public abstract partial class NinnelPipelineFactory
		: NinnelComponent0,
			INinnelPipelineFactory
	{
		#region Methods/Operators

		protected abstract ValueTask<INinnelPipeline> CoreCreatePipelineAsync(Type ninnelPipelineType, CancellationToken cancellationToken = default);

		public ValueTask<INinnelPipeline> CreatePipelineAsync(Type ninnelPipelineType, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelPipelineType == null)
				throw new ArgumentNullException(nameof(ninnelPipelineType));

			try
			{
				return this.CoreCreatePipelineAsync(ninnelPipelineType, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The pipeline factory failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}