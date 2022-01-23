/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Hosting.Tool;
using WellEngineered.Solder.Executive;

namespace WellEngineered.Ninnel.Host.Cli
{
	/// <summary>
	/// Entry point static class for the application.
	/// </summary>
	public static partial class Program
	{
		#region Methods/Operators

		[STAThread]
		public static async Task<int> MainAsync(string[] args)
		{
			return await ExecutableApplication.ResolveRunAsync<NinnelConsoleApplication>(args);
		}

		#endregion
	}
}