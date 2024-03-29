/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Primitives.Configuration
{
	public abstract partial class NinnelConfiguration
		: SolderConfiguration,
			INinnelConfiguration
	{
		#region Constructors/Destructors

		protected NinnelConfiguration()
		{
		}

		#endregion

		#region Methods/Operators

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			yield break;
		}

		#endregion
	}
}