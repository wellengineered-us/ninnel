/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public abstract partial class NinnelToolHost
		: NinnelHost<ToolHostConfiguration>,
			INinnelToolHost
	{
		#region Methods/Operators

		protected abstract ValueTask CoreHostAsync(IDictionary<string, IList<object>> arguments, IDictionary<string, IList<object>> properties, CancellationToken cancellationToken = default);

		public ValueTask HostAsync(IDictionary<string, IList<object>> arguments, IDictionary<string, IList<object>> properties, CancellationToken cancellationToken = default)
		{
			if ((object)arguments == null)
				throw new ArgumentNullException(nameof(arguments));

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			try
			{
				return this.CoreHostAsync(arguments, properties, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The tool host failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}
#endif