/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Hosting
{
	public abstract partial class NinnelHostFactory
		: NinnelComponent0,
			INinnelHostFactory
	{
		#region Methods/Operators

		protected abstract ValueTask<INinnelHost> CoreCreateHostAsync(CancellationToken cancellationToken = default);

		public ValueTask<INinnelHost> CreateHostAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				return this.CoreCreateHostAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The host factory failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}