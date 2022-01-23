/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Primitives;
using WellEngineered.Solder.Serialization.JsonNet;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public sealed class JsonFileToolHostFactory
		: NinnelToolHostFactory
	{
		#region Constructors/Destructors

		public JsonFileToolHostFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected override INinnelHost CoreCreateHost()
		{
			return null;
		}

		protected override INinnelToolHost CoreCreateHost(ToolHostConfiguration toolHostConfiguration)
		{
			IEnumerable<IMessage> messages;

			if ((object)toolHostConfiguration == null)
				throw new ArgumentNullException(nameof(toolHostConfiguration));

			messages = toolHostConfiguration.Validate("Host");

			if ((object)messages != null)
			{
				int count = 0;
				foreach (IMessage message in messages)
				{
					if (message == null)
						continue;

					Console.Out.WriteLine(string.Format("{0}[{1}] => {2}", message.Severity, (count + 1), message.Description));

					count++;
				}

				if (count > 0)
					throw new NinnelException(string.Format("Tool host configuration validation failed with error count: {0}", count));
			}

			// +++

			IDependencyManager dependencyManager;

			Type ninnelToolHostType;
			INinnelToolHost ninnelToolHost;

			dependencyManager = AssemblyDomain.Default.DependencyManager;
			ninnelToolHostType = toolHostConfiguration.GetHostType();

			if ((object)ninnelToolHostType == null)
				throw new NinnelException(string.Format("Failed to load tool host type from AQTN: '{0}'.", toolHostConfiguration.HostAssemblyQualifiedTypeName));

			bool autoWire;
			if (autoWire = (toolHostConfiguration.HostAutoWire ?? true))
				ninnelToolHost = ninnelToolHostType.ResolveAutoWireAssignableToTargetType<INinnelToolHost>(dependencyManager);
			else
				ninnelToolHost = ninnelToolHostType.CreateInstanceAssignableToTargetType<INinnelToolHost>();

			if ((object)ninnelToolHost == null)
				throw new NinnelException(string.Format("Failed to instantiate tool host type: '{0}', auto-wire: {1}.", toolHostConfiguration.HostAssemblyQualifiedTypeName, autoWire));

			ninnelToolHost.Configuration = toolHostConfiguration;
			ninnelToolHost.Create();

			return ninnelToolHost;
		}

		protected override INinnelToolHost CoreCreateHost(Uri toolHostConfigUri)
		{
			ToolHostConfiguration toolHostConfiguration;

			if ((object)toolHostConfigUri == null)
				throw new ArgumentNullException(nameof(toolHostConfigUri));

			var json = new NativeJsonSerializationStrategy();
			toolHostConfiguration = json.DeserializeObjectFromFile<ToolHostConfiguration>(toolHostConfigUri.LocalPath);

			if ((object)toolHostConfiguration == null)
				throw new NinnelException(string.Format("Failed to deserialize tool host configuration from URI: '{0}'.", toolHostConfigUri));

			return this.CoreCreateHost(toolHostConfiguration);
		}

		protected override ValueTask<INinnelHost> CoreCreateHostAsync(CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected override ValueTask<INinnelToolHost> CoreCreateHostAsync(ToolHostConfiguration toolHostConfiguration, CancellationToken cancellationToken = default)
		{
			if ((object)toolHostConfiguration == null)
				throw new ArgumentNullException(nameof(toolHostConfiguration));

			return default;
		}

		protected override ValueTask<INinnelToolHost> CoreCreateHostAsync(Uri toolHostConfigUri, CancellationToken cancellationToken = default)
		{
			if ((object)toolHostConfigUri == null)
				throw new ArgumentNullException(nameof(toolHostConfigUri));

			return default;
		}

		#endregion
	}
}