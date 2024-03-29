/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Transport
{
	public abstract partial class NinnelPipelineFactory
		: NinnelComponent0,
			INinnelPipelineFactory
	{
		#region Constructors/Destructors

		protected NinnelPipelineFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract INinnelPipeline CoreCreatePipeline(Type ninnelPipelineType);

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
				throw new NinnelException(string.Format("The pipeline factory failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}