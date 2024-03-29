/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Solder.Executive;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Injection.Resolutions;
using WellEngineered.Solder.Primitives;
using WellEngineered.Solder.Utilities.AppSettings;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public sealed partial class NinnelConsoleApplication
		: ExecutableApplication
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public NinnelConsoleApplication([DependencyInjection] IAppSettingsFacade appSettingsFacade,
			[DependencyInjection] AssemblyInformation assemblyInformation)
			: base(appSettingsFacade, assemblyInformation)
		{
		}

		#endregion

		#region Fields/Constants

		private const string ARG_HOST_CONFIG_FILE_ITEM = "hostconfigfile";
		private const string ARG_PROPERTY_ITEM = "property";

		#endregion

		#region Properties/Indexers/Events

		protected override TimeSpan ProcessExitDelayTimeSpan
		{
			get
			{
				return TimeSpan.FromSeconds(30);
			}
		}

		#endregion

		#region Methods/Operators

		protected override IDictionary<string, ArgumentSpec> CoreGetArgumentMap()
		{
			IDictionary<string, ArgumentSpec> argumentMap;

			argumentMap = new Dictionary<string, ArgumentSpec>();
			argumentMap.Add(ARG_HOST_CONFIG_FILE_ITEM, new ArgumentSpec<string>(true, true));
			argumentMap.Add(ARG_PROPERTY_ITEM, new ArgumentSpec<string>(false, false));

			return argumentMap;
		}

		protected override int CoreRun(IDictionary<string, IList<object>> arguments)
		{
			IList<object> argumentValues;

			IDictionary<string, IList<object>> properties;
			object propertyValue;
			IList<object> propertyValues;

			bool hasProperties;

			if ((object)arguments == null)
				throw new ArgumentNullException(nameof(arguments));

			// required
			properties = new Dictionary<string, IList<object>>();
			hasProperties = arguments.TryGetValue(ARG_PROPERTY_ITEM, out argumentValues);

			if (hasProperties)
			{
				if ((object)argumentValues != null)
				{
					foreach (string argumentValue in argumentValues)
					{
						string key, value;

						if (!TryParseCommandLineArgumentProperty(argumentValue, out key, out value))
							continue;

						if (!properties.TryGetValue(key, out propertyValues))
							properties.Add(key, propertyValues = new List<object>());

						// duplicate values are ignored
						//if (propertyValues.Contains(value))
						//continue;

						propertyValues.Add(value);
					}
				}
			}

			//+++

			string toolHostConfigFilePath = (string)arguments[ARG_HOST_CONFIG_FILE_ITEM].SingleOrDefault();

			if ((object)toolHostConfigFilePath == null)
				throw new NinnelException(string.Format("Failed to load tool host configuration from command line argument: '{0}'.", ARG_HOST_CONFIG_FILE_ITEM));

			toolHostConfigFilePath = Path.GetFullPath(toolHostConfigFilePath);
			string toolHostConfigUrl = string.Format("{0}:///{1}", Uri.UriSchemeFile, toolHostConfigFilePath);

			if (!Uri.TryCreate(toolHostConfigUrl, UriKind.RelativeOrAbsolute, out Uri toolHostConfigUri))
				throw new NinnelException(string.Format("Failed to load tool host configuration from command line argument: '{0}'.", toolHostConfigUrl));

			if (!toolHostConfigUri.IsFile)
				throw new NinnelException(string.Format("Failed to load tool host configuration from URI: '{0}'.", toolHostConfigUri));

			if (toolHostConfigUri.Scheme != Uri.UriSchemeFile)
				throw new NinnelException(string.Format("Failed to load tool host configuration from URI: '{0}'.", toolHostConfigUri));

			using (INinnelToolHostFactory ninnelToolHostFactory = new JsonFileToolHostFactory())
			{
				ninnelToolHostFactory.Create();

				using (INinnelToolHost ninnelToolHost = ninnelToolHostFactory.CreateToolHost(toolHostConfigUri))
				{
					if (!ninnelToolHost.IsCreated ||
						(object)ninnelToolHost.Configuration == null)
						throw new NinnelException();

					AssemblyDomain
						.Default
						.DependencyManager
						.AddResolution<INinnelToolHost>(string.Empty, false, new SingletonWrapperDependencyResolution<INinnelToolHost>(new InstanceDependencyResolution<INinnelToolHost>(ninnelToolHost)));

					ninnelToolHost.Host(arguments, properties);
				}
			}

			return 0;
		}

		protected override void CoreSignal(Signal signal)
		{
			IDependencyManager dependencyManager;
			INinnelToolHost ninnelToolHost;

			if (signal == Signal.SignalEnvExit)
				return;

			dependencyManager = AssemblyDomain.Default.DependencyManager;

			if (dependencyManager.HasTypeResolution<INinnelToolHost>(string.Empty, false))
			{
				AssemblyDomain.Default.ResourceManager.Print(Guid.Empty, signal.ToString());

				ninnelToolHost = dependencyManager.ResolveDependency<INinnelToolHost>(string.Empty, false);

				if ((object)ninnelToolHost != null)
					ninnelToolHost.Cancel();

				dependencyManager.RemoveResolution<INinnelToolHost>(string.Empty, false);
			}
		}

		#endregion
	}
}