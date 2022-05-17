/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Component;
using WellEngineered.Solder.Injection;

namespace WellEngineered.Ninnel.Primitives.Component
{
	public abstract partial class NinnelComponentFactory
		: SolderComponent0,
			INinnelComponentFactory
	{
		#region Methods/Operators

		protected abstract TNinnelComponent CoreCreateNinnelComponent<TNinnelComponent>(IDependencyManager dependencyManager, Type ninnelComponentType, bool autoWire)
			where TNinnelComponent : INinnelComponent0;

		public TNinnelComponent CreateNinnelComponent<TNinnelComponent>(IDependencyManager dependencyManager, Type ninnelComponentType, bool autoWire)
			where TNinnelComponent : INinnelComponent0
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)ninnelComponentType == null)
				throw new ArgumentNullException(nameof(ninnelComponentType));

			try
			{
				return this.CoreCreateNinnelComponent<TNinnelComponent>(dependencyManager, ninnelComponentType, autoWire);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The component factory failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}