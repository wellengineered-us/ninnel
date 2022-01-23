/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Context;
using WellEngineered.Ninnel.Primitives;

namespace WellEngineered.Ninnel.Transport
{
	public abstract partial class NinnelPipeline
		: NinnelTransport<PipelineConfiguration>,
			INinnelPipeline
	{
		#region Methods/Operators

		public ValueTask<INinnelContext> CloneContextAsync(INinnelContext ninnelContext, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelContext == null)
				throw new ArgumentNullException(nameof(ninnelContext));

			try
			{
				return this.CoreCloneContextAsync(ninnelContext, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The pipeline failed (see inner exception)."), ex);
			}
		}

		protected abstract ValueTask<INinnelContext> CoreCloneContextAsync(INinnelContext ninnelContext, CancellationToken cancellationToken = default);

		protected abstract ValueTask<INinnelContext> CoreCreateContextAsync(CancellationToken cancellationToken = default);

		protected abstract ValueTask<long> CoreExecuteAsync(INinnelContext ninnelContext, CancellationToken cancellationToken = default);

		public ValueTask<INinnelContext> CreateContextAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				return this.CoreCreateContextAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The pipeline failed (see inner exception)."), ex);
			}
		}

		public ValueTask<long> ExecuteAsync(INinnelContext ninnelContext, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelContext == null)
				throw new ArgumentNullException(nameof(ninnelContext));

			try
			{
				return this.CoreExecuteAsync(ninnelContext, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The pipeline failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}