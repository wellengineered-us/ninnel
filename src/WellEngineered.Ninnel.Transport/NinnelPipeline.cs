/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Context;
using WellEngineered.Ninnel.Primitives;

namespace WellEngineered.Ninnel.Transport
{
	public abstract partial class NinnelPipeline
		: NinnelTransport<PipelineConfiguration>,
			INinnelPipeline
	{
		#region Constructors/Destructors

		protected NinnelPipeline()
		{
		}

		#endregion

		#region Methods/Operators

		public INinnelContext CloneContext(INinnelContext ninnelContext)
		{
			if ((object)ninnelContext == null)
				throw new ArgumentNullException(nameof(ninnelContext));

			try
			{
				return this.CoreCloneContext(ninnelContext);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The pipeline failed (see inner exception)."), ex);
			}
		}

		protected abstract INinnelContext CoreCloneContext(INinnelContext ninnelContext);

		protected abstract INinnelContext CoreCreateContext();

		protected abstract long CoreExecute(INinnelContext ninnelContext);

		public INinnelContext CreateContext()
		{
			try
			{
				return this.CoreCreateContext();
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The pipeline failed (see inner exception)."), ex);
			}
		}

		public long Execute(INinnelContext ninnelContext)
		{
			if ((object)ninnelContext == null)
				throw new ArgumentNullException(nameof(ninnelContext));

			try
			{
				return this.CoreExecute(ninnelContext);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The pipeline failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}