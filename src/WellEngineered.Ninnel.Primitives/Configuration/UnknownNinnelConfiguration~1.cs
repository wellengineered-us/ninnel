/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Component;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Primitives.Configuration
{
	public class UnknownNinnelConfiguration<TNinnelSpecification>
		: UnknownSolderConfiguration<TNinnelSpecification>,
			IUnknownNinnelConfiguration<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Constructors/Destructors

		public UnknownNinnelConfiguration(IUnknownSolderConfiguration that)
			: base(that)
		{
		}

		#endregion
	}
}