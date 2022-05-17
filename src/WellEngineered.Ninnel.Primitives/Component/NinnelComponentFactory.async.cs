/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Component;
using WellEngineered.Solder.Injection;

namespace WellEngineered.Ninnel.Primitives.Component
{
	public partial class NinnelComponentFactory
		: SolderComponent0,
			INinnelComponentFactory
	{
		#region Methods/Operators

		protected abstract ValueTask<TNinnelComponent> CoreCreateNinnelComponentAsync<TNinnelComponent>(IDependencyManager dependencyManager, Type ninnelComponentType, bool autoWire, CancellationToken cancellationToken = default)
			where TNinnelComponent : INinnelComponent0;

		public async ValueTask<TNinnelComponent> CreateNinnelComponentAsync<TNinnelComponent>(IDependencyManager dependencyManager, Type ninnelComponentType, bool autoWire, CancellationToken cancellationToken = default)
			where TNinnelComponent : INinnelComponent0
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)ninnelComponentType == null)
				throw new ArgumentNullException(nameof(ninnelComponentType));

			try
			{
				return await this.CoreCreateNinnelComponentAsync<TNinnelComponent>(dependencyManager, ninnelComponentType, autoWire, cancellationToken);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The component factory failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}
#endif