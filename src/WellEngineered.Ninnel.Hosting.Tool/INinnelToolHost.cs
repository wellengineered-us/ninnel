/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public partial interface INinnelToolHost
		: INinnelHost<ToolHostConfiguration>
	{
		#region Methods/Operators

		void Host(IDictionary<string, IList<object>> arguments,
			IDictionary<string, IList<object>> properties);

		#endregion
	}
}