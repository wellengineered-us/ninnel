/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public abstract class NinnelToolHostFactory
		: NinnelHostFactory,
			INinnelToolHostFactory
	{
		#region Constructors/Destructors

		public NinnelToolHostFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract INinnelToolHost CoreCreateHost(ToolHostConfiguration toolHostConfiguration);

		protected abstract INinnelToolHost CoreCreateHost(Uri toolHostConfigUri);

		protected abstract ValueTask<INinnelToolHost> CoreCreateHostAsync(ToolHostConfiguration toolHostConfiguration, CancellationToken cancellationToken = default);

		protected abstract ValueTask<INinnelToolHost> CoreCreateHostAsync(Uri toolHostConfigUri, CancellationToken cancellationToken = default);

		public INinnelToolHost CreateToolHost(ToolHostConfiguration toolHostConfiguration)
		{
			if ((object)toolHostConfiguration == null)
				throw new ArgumentNullException(nameof(toolHostConfiguration));

			try
			{
				return this.CoreCreateHost(toolHostConfiguration);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The tool host factory failed (see inner exception)."), ex);
			}
		}

		public INinnelToolHost CreateToolHost(Uri toolHostConfigUri)
		{
			if ((object)toolHostConfigUri == null)
				throw new ArgumentNullException(nameof(toolHostConfigUri));

			try
			{
				return this.CoreCreateHost(toolHostConfigUri);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The tool host factory failed (see inner exception)."), ex);
			}
		}

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