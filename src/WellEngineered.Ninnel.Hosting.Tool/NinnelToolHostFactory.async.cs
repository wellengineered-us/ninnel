/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public abstract partial class NinnelToolHostFactory
		: NinnelHostFactory,
			INinnelToolHostFactory
	{
		#region Methods/Operators

		protected abstract ValueTask<INinnelToolHost> CoreCreateHostAsync(ToolHostConfiguration toolHostConfiguration, CancellationToken cancellationToken = default);

		protected abstract ValueTask<INinnelToolHost> CoreCreateHostAsync(Uri toolHostConfigUri, CancellationToken cancellationToken = default);

		public ValueTask<INinnelToolHost> CreateToolHostAsync(ToolHostConfiguration toolHostConfiguration, CancellationToken cancellationToken = default)
		{
			if ((object)toolHostConfiguration == null)
				throw new ArgumentNullException(nameof(toolHostConfiguration));

			try
			{
				return this.CoreCreateHostAsync(toolHostConfiguration, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The tool host factory failed (see inner exception)."), ex);
			}
		}

		public ValueTask<INinnelToolHost> CreateToolHostAsync(Uri toolHostConfigUri, CancellationToken cancellationToken = default)
		{
			if ((object)toolHostConfigUri == null)
				throw new ArgumentNullException(nameof(toolHostConfigUri));

			try
			{
				return this.CoreCreateHostAsync(toolHostConfigUri, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The tool host factory failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}
#endif