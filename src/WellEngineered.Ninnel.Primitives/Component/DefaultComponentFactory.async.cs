/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Injection;

namespace WellEngineered.Ninnel.Primitives.Component
{
	public sealed partial class DefaultComponentFactory
		: NinnelComponentFactory
	{
		#region Methods/Operators

		protected override async ValueTask<TNinnelComponent> CoreCreateNinnelComponentAsync<TNinnelComponent>(IDependencyManager dependencyManager, Type ninnelComponentType, bool autoWire, CancellationToken cancellationToken = default)
		{
			TNinnelComponent ninnelComponent;

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)ninnelComponentType == null)
				throw new ArgumentNullException(nameof(ninnelComponentType));

			ninnelComponent = ninnelComponentType.GetObjectAssignableToTargetType<TNinnelComponent>(dependencyManager, autoWire);

			if ((object)ninnelComponent == null)
				throw new NinnelException(string.Format("Failed to instantiate ninnel component type: '{0}', auto-wire: {1}.", ninnelComponentType.FullName, autoWire));

			await Task.CompletedTask;
			return ninnelComponent;
		}

		#endregion
	}
}
#endif