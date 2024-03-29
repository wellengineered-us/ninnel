/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public partial interface INinnelToolHostFactory : INinnelHostFactory
	{
		#region Methods/Operators

		INinnelToolHost CreateToolHost(ToolHostConfiguration toolHostConfiguration);

		INinnelToolHost CreateToolHost(Uri toolHostConfigFileUri);

		#endregion
	}
}