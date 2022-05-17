/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Configuration
{
	public partial class HostConfiguration
		: NinnelConfiguration
	{
		#region Constructors/Destructors

		public HostConfiguration()
		{
			this.pipelineConfigurations = new SolderConfigurationCollection<PipelineConfiguration>(this);
		}

		public HostConfiguration(ISolderConfigurationCollection<PipelineConfiguration> pipelineConfigurations)
		{
			if ((object)pipelineConfigurations == null)
				throw new ArgumentNullException(nameof(pipelineConfigurations));

			this.pipelineConfigurations = pipelineConfigurations;
		}

		#endregion

		#region Fields/Constants

		private static readonly Version currentConfigurationVersion = Version.Parse("1.0.0");
		private static readonly Version currentEngineVersion = Version.Parse("0.9.0");
		private static readonly Version minimumConfigurationVersion = Version.Parse("1.0.0");
		private readonly ISolderConfigurationCollection<PipelineConfiguration> pipelineConfigurations;
		private bool? componentAutoWire;
		private string configurationVersion;
		private string contextAssemblyQualifiedTypeName;
		private string hostAssemblyQualifiedTypeName;
		private string targetEngineVersion;

		#endregion

		#region Properties/Indexers/Events

		public static Version CurrentConfigurationVersion
		{
			get
			{
				return currentConfigurationVersion;
			}
		}

		public static Version CurrentEngineVersion
		{
			get
			{
				return currentEngineVersion;
			}
		}

		public static Version MinimumConfigurationVersion
		{
			get
			{
				return minimumConfigurationVersion;
			}
		}

		public ISolderConfigurationCollection<PipelineConfiguration> PipelineConfigurations
		{
			get
			{
				return this.pipelineConfigurations;
			}
		}

		public bool? ComponentAutoWire
		{
			get
			{
				return this.componentAutoWire;
			}
			set
			{
				this.componentAutoWire = value;
			}
		}

		public string ConfigurationVersion
		{
			get
			{
				return this.configurationVersion;
			}
			set
			{
				this.configurationVersion = value;
			}
		}

		public string ContextAssemblyQualifiedTypeName
		{
			get
			{
				return this.contextAssemblyQualifiedTypeName;
			}
			set
			{
				this.contextAssemblyQualifiedTypeName = value;
			}
		}

		public string HostAssemblyQualifiedTypeName
		{
			get
			{
				return this.hostAssemblyQualifiedTypeName;
			}
			set
			{
				this.hostAssemblyQualifiedTypeName = value;
			}
		}

		public string TargetEngineVersion
		{
			get
			{
				return this.targetEngineVersion;
			}
			set
			{
				this.targetEngineVersion = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override IEnumerable<IMessage> CoreValidate(object context)
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
				var childMessages = this.PipelineConfigurations.Validate("Pipeline");

				foreach (var childMessage in childMessages)
				{
					yield return childMessage;
				}
			}
		}

		public Type GetContextType()
		{
			return ReflectionExtensions.GetTypeFromAssemblyQualifiedTypeName(this.ContextAssemblyQualifiedTypeName);
		}

		public Type GetHostType()
		{
			return ReflectionExtensions.GetTypeFromAssemblyQualifiedTypeName(this.HostAssemblyQualifiedTypeName);
		}

		#endregion
	}
}