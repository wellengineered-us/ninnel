/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

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
		#region Constructors/Destructors

		protected NinnelHost()
		{
		}

		#endregion

		#region Methods/Operators

		public void Cancel()
		{
			try
			{
				this.CoreCancel();
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		protected abstract void CoreCancel();

		protected abstract INinnelPipeline CoreCreatePipeline(Type ninnelPipelineType);

		protected abstract void CoreHost();

		public INinnelPipeline CreatePipeline(Type ninnelPipelineType)
		{
			if ((object)ninnelPipelineType == null)
				throw new ArgumentNullException(nameof(ninnelPipelineType));

			try
			{
				return this.CoreCreatePipeline(ninnelPipelineType);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		public void Host()
		{
			try
			{
				this.CoreHost();
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}