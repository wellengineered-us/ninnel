/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Hosting.Tool;
using WellEngineered.Solder.Executive;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Injection.Resolutions;

namespace WellEngineered.Ninnel.Host.Cli
{
	/// <summary>
	/// Entry point static class for the application.
	/// </summary>
	public static partial class Program
	{
		#region Methods/Operators

		[STAThread]
		public static int Main(string[] args)
		{
			return ExecutableApplication.ResolveRun<NinnelConsoleApplication>(args);
		}

		[DependencyMagicMethod]
		public static void OnDependencyMagic(IDependencyManager dependencyManager)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			dependencyManager.AddResolution<NinnelConsoleApplication>(string.Empty, false, new SingletonWrapperDependencyResolution<NinnelConsoleApplication>(new TransientActivatorAutoWiringDependencyResolution<NinnelConsoleApplication>()));
		}

		#endregion
	}
}