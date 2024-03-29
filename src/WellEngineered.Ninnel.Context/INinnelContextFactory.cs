/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Context
{
	public partial interface INinnelContextFactory
		: INinnelComponent0
	{
		#region Methods/Operators

		INinnelContext CloneContext(INinnelContext ninnelContext);

		INinnelContext CreateContext(Type ninnelContextType);

		#endregion
	}
}