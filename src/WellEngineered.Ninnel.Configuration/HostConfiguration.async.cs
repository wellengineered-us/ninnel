/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Configuration
{
	public partial class HostConfiguration
		: NinnelConfiguration
	{
		#region Methods/Operators

		protected override async IAsyncEnumerable<IMessage> CoreValidateAsync(object context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			Type ninnelHostType;
			//Type ninnelContextType;

			if ((object)MinimumConfigurationVersion == null ||
				(object)CurrentConfigurationVersion == null)
				throw new NinnelException(string.Format("{0} ({1}) and/or {2} ({3}) is null.", nameof(MinimumConfigurationVersion), MinimumConfigurationVersion, nameof(CurrentConfigurationVersion), CurrentConfigurationVersion));

			if (MinimumConfigurationVersion > CurrentConfigurationVersion)
				throw new NinnelException(string.Format("{0} ({1}) is greater than {2} ({3}).", nameof(MinimumConfigurationVersion), MinimumConfigurationVersion, nameof(CurrentConfigurationVersion), CurrentConfigurationVersion));

			if (string.IsNullOrEmpty(this.ConfigurationVersion) ||
				!Version.TryParse(this.ConfigurationVersion, out Version cv))
				yield return new Message(string.Empty, string.Format("{0} configuration error: missing or invalid property {1}.", context, nameof(this.ConfigurationVersion)), Severity.Error);
			else if ((object)cv == null ||
					cv < MinimumConfigurationVersion ||
					cv > CurrentConfigurationVersion)
				yield return new Message(string.Empty, string.Format("{0} configuration error: value of property {1} ({2}) is out of range [{3} ({4}) , {5} ({6})].", context, nameof(this.ConfigurationVersion), this.ConfigurationVersion,
					nameof(MinimumConfigurationVersion), MinimumConfigurationVersion, nameof(CurrentConfigurationVersion), CurrentConfigurationVersion), Severity.Error);

			if (string.IsNullOrWhiteSpace(this.HostAssemblyQualifiedTypeName))
				yield return new Message(string.Empty, string.Format("{0} assembly qualified type name is required.", context), Severity.Error);
			else
			{
				ninnelHostType = this.GetHostType();

				if ((object)ninnelHostType == null)
					yield return new Message(string.Empty, string.Format("{0} assembly qualified type name failed to load.", context), Severity.Error);
				/*else if (!typeof(INinnelHost).IsAssignableFrom(ninnelHostType))
					yield return new Message(string.Empty, string.Format("{0} assembly qualified type name loaded an unrecognized type.", context), Severity.Error);
				else
				{
					ninnelHost = ReflectionExtensions.CreateInstanceAssignableToTargetType<INinnelHost>(ninnelHostType);

					if ((object)ninnelHost == null)
						yield return new Message(string.Empty, string.Format("{0} assembly qualified type name failed to instantiate type.", context), Severity.Error);
				}*/
			}

			/*if (string.IsNullOrWhiteSpace(this.ContextAssemblyQualifiedTypeName))
				yield return new Message(string.Empty, string.Format("{0} context assembly qualified type name is required.", context), Severity.Error);
			else
			{
				ninnelContextType = this.GetContextType();

				if ((object)ninnelContextType == null)
					yield return new Message(string.Empty, string.Format("{0} context assembly qualified type name failed to load.", context), Severity.Error);
				else if (!typeof(INinnelContext).IsAssignableFrom(ninnelContextType))
					yield return new Message(string.Empty, string.Format("{0} context assembly qualified type name loaded an unrecognized type.", context), Severity.Error);
				else
				{
					// new-ing up via default public constructor should be low friction
					ninnelContext = ReflectionExtensions.CreateInstanceAssignableToTargetType<INinnelContext>(ninnelContextType);

					if ((object)ninnelContext == null)
						yield return new Message(string.Empty, string.Format("{0} context assembly qualified type name failed to instantiate type.", context), Severity.Error);
				}
			}*/

			if ((object)this.PipelineConfigurations == null)
				yield return new Message(string.Empty, string.Format("Pipeline configurations are required."), Severity.Error);
			else
			{
				var childMessages = this.PipelineConfigurations.ValidateAsync("Pipeline", cancellationToken);

				await foreach (var childMessage in childMessages.WithCancellation(cancellationToken))
				{
					yield return childMessage;
				}
			}
		}

		#endregion
	}
}
#endif