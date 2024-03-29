/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Transport;

namespace WellEngineered.Ninnel.Hosting
{
	public abstract partial class NinnelHost<THostConfiguration>
		: NinnelComponent<THostConfiguration>,
			INinnelHost<THostConfiguration>
		where THostConfiguration : HostConfiguration
	{
		#region Methods/Operators

		public ValueTask CancelAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				return this.CoreCancelAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		protected abstract ValueTask CoreCancelAsync(CancellationToken cancellationToken = default);

		protected abstract ValueTask<INinnelPipeline> CoreCreatePipelineAsync(Type ninnelPipelineType, CancellationToken cancellationToken = default);

		protected abstract ValueTask CoreHostAsync(CancellationToken cancellationToken = default);

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
				throw new NinnelException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		public ValueTask HostAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				return this.CoreHostAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}
#endif