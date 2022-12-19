/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Hosting.Tool;
using WellEngineered.Ninnel.Primitives.Component;
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

#if ASYNC_MAIN_ENTRY_POINT
		[STAThread]
		public static async Task<int> Main(string[] args)
		{
			return await ExecutableApplication.ResolveRunAsync<NinnelConsoleApplication>(args);
		}

		[AsyncDependencyMagicMethod]
		public static async ValueTask OnDependencyMagicAsync(IDependencyManager dependencyManager, CancellationToken cancellationToken = default)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			// the only hard-coded dependencies
			await dependencyManager.AddResolutionAsync<INinnelComponentFactory>(string.Empty, false, new SingletonWrapperDependencyResolution<INinnelComponentFactory>(new InstanceDependencyResolution<INinnelComponentFactory>(new DefaultComponentFactory())), cancellationToken);
			await dependencyManager.AddResolutionAsync<NinnelConsoleApplication>(string.Empty, false, new SingletonWrapperDependencyResolution<NinnelConsoleApplication>(new TransientActivatorAutoWiringDependencyResolution<NinnelConsoleApplication>()), cancellationToken);
		}
#endif
		#endregion
	}
}
#endif