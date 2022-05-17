/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Solder.Executive;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Injection.Resolutions;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public sealed partial class NinnelConsoleApplication
		: ExecutableApplication
	{
		#region Methods/Operators

		protected async override ValueTask<int> CoreRunAsync(IDictionary<string, IList<object>> arguments, CancellationToken cancellationToken = default)
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

			await using (INinnelToolHostFactory ninnelToolHostFactory = new JsonFileToolHostFactory())
			{
				await ninnelToolHostFactory.CreateAsync();

				await using (INinnelToolHost ninnelToolHost = await ninnelToolHostFactory.CreateToolHostAsync(toolHostConfigUri, cancellationToken))
				{
					if (!ninnelToolHost.IsAsyncCreated ||
						(object)ninnelToolHost.Configuration == null)
						throw new NinnelException();

					await AssemblyDomain
						.Default
						.DependencyManager
						.AddResolutionAsync<INinnelToolHost>(string.Empty, false, new SingletonWrapperDependencyResolution<INinnelToolHost>(new InstanceDependencyResolution<INinnelToolHost>(ninnelToolHost)), cancellationToken);

					await ninnelToolHost.HostAsync(arguments, properties, cancellationToken);
				}
			}

			return 0;
		}

		protected override async ValueTask CoreSignalAsync(Signal signal, CancellationToken cancellationToken = default)
		{
			IDependencyManager dependencyManager;
			INinnelToolHost ninnelToolHost;

			if (signal == Signal.SignalEnvExit)
				return;

			dependencyManager = AssemblyDomain.Default.DependencyManager;

			if (await dependencyManager.HasTypeResolutionAsync<INinnelToolHost>(string.Empty, false, cancellationToken))
			{
				await AssemblyDomain.Default.ResourceManager.PrintAsync(Guid.Empty, signal.ToString(), cancellationToken);

				ninnelToolHost = await dependencyManager.ResolveDependencyAsync<INinnelToolHost>(string.Empty, false, cancellationToken);

				if ((object)ninnelToolHost != null)
					await ninnelToolHost.CancelAsync(cancellationToken);

				await dependencyManager.RemoveResolutionAsync<INinnelToolHost>(string.Empty, false, cancellationToken);
			}
		}

		#endregion
	}
}
#endif