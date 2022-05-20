/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Primitives;
using WellEngineered.Solder.Serialization.JsonNet;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public sealed partial class JsonFileToolHostFactory
		: NinnelToolHostFactory
	{
		#region Methods/Operators

		protected override async ValueTask<INinnelHost> CoreCreateHostAsync(CancellationToken cancellationToken = default)
		{
			return await this.CoreCreateHostAsync(new ToolHostConfiguration(), cancellationToken);
		}

		protected override async ValueTask<INinnelToolHost> CoreCreateHostAsync(ToolHostConfiguration toolHostConfiguration, CancellationToken cancellationToken = default)
		{
			IAsyncEnumerable<IMessage> messages;

			if ((object)toolHostConfiguration == null)
				throw new ArgumentNullException(nameof(toolHostConfiguration));

			messages = toolHostConfiguration.ValidateAsync("Host", cancellationToken);

			if ((object)messages != null)
			{
				int count = 0;
				await foreach (IMessage message in messages.WithCancellation(cancellationToken))
				{
					if (message == null)
						continue;

					await Console.Out.WriteLineAsync(string.Format("{0}[{1}] => {2}", message.Severity, (count + 1), message.Description));

					count++;
				}

				if (count > 0)
					throw new NinnelException(string.Format("Tool host configuration validation failed with error count: {0}", count));
			}

			// +++

			IDependencyManager dependencyManager;

			Type ninnelToolHostType;
			INinnelToolHost ninnelToolHost;
			bool autoWire;

			dependencyManager = AssemblyDomain.Default.DependencyManager;
			ninnelToolHostType = toolHostConfiguration.GetHostType();

			if ((object)ninnelToolHostType == null)
				throw new NinnelException(string.Format("Failed to load tool host type from AQTN: '{0}'.", toolHostConfiguration.HostAssemblyQualifiedTypeName));

			autoWire = toolHostConfiguration.ComponentAutoWire ?? true;
			// TODO - Fix this
			ninnelToolHost = await new DefaultComponentFactory().CreateNinnelComponentAsync<INinnelToolHost>(AssemblyDomain.Default.DependencyManager, ninnelToolHostType, autoWire, null, true, cancellationToken);

			if ((object)ninnelToolHost == null)
				throw new NinnelException(string.Format("Failed to instantiate tool host type: '{0}', auto-wire: {1}.", toolHostConfiguration.HostAssemblyQualifiedTypeName, autoWire));

			ninnelToolHost.Configuration = toolHostConfiguration;
			await ninnelToolHost.CreateAsync();

			return ninnelToolHost;
		}

		protected override async ValueTask<INinnelToolHost> CoreCreateHostAsync(Uri toolHostConfigUri, CancellationToken cancellationToken = default)
		{
			ToolHostConfiguration toolHostConfiguration;

			if ((object)toolHostConfigUri == null)
				throw new ArgumentNullException(nameof(toolHostConfigUri));

			var json = new NativeJsonSerializationStrategy();
			toolHostConfiguration = await json.DeserializeObjectFromFileAsync<ToolHostConfiguration>(toolHostConfigUri.LocalPath, cancellationToken);

			if ((object)toolHostConfiguration == null)
				throw new NinnelException(string.Format("Failed to deserialize tool host configuration from URI: '{0}'.", toolHostConfigUri));

			return await this.CoreCreateHostAsync(toolHostConfiguration, cancellationToken);
		}

		#endregion
	}
}
#endif