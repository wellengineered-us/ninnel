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
	public partial interface INinnelComponentFactory
		: ISolderComponent0
	{
		#region Methods/Operators

		ValueTask<TNinnelComponent> CreateNinnelComponentAsync<TNinnelComponent>(IDependencyManager dependencyManager, Type ninnelComponentType, bool autoWire, CancellationToken cancellationToken = default)
			where TNinnelComponent : INinnelComponent0;

		#endregion
	}
}
#endif