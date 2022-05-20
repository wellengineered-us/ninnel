/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Injection;

namespace WellEngineered.Ninnel.Primitives.Component
{
	public sealed partial class DefaultComponentFactory
		: NinnelComponentFactory
	{
		#region Constructors/Destructors

		public DefaultComponentFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected override TNinnelComponent CoreCreateNinnelComponent<TNinnelComponent>(IDependencyManager dependencyManager, Type ninnelComponentType, bool autoWire, string selectorKey = null, bool throwOnError = true)
		{
			TNinnelComponent ninnelComponent;

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)ninnelComponentType == null)
				throw new ArgumentNullException(nameof(ninnelComponentType));

			ninnelComponent = ninnelComponentType.GetObjectAssignableToTargetType<TNinnelComponent>(dependencyManager, autoWire, selectorKey, throwOnError);

			if ((object)ninnelComponent == null)
				throw new NinnelException(string.Format("Failed to instantiate ninnel component type: '{0}', auto-wire: {1}.", ninnelComponentType.FullName, autoWire));

			return ninnelComponent;
		}

		#endregion
	}
}