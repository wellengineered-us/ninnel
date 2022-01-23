/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;

using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Configuration
{
	public class PipelineConfiguration : NinnelConfiguration
	{
		#region Constructors/Destructors

		public PipelineConfiguration()
		{
			this.intermediateStationConfigurations = new SolderConfigurationCollection<UnknownNinnelConfiguration>(this);
		}

		public PipelineConfiguration(ISolderConfigurationCollection<UnknownNinnelConfiguration> intermediateStationConfigurations)
		{
			if ((object)intermediateStationConfigurations == null)
				throw new ArgumentNullException(nameof(intermediateStationConfigurations));

			this.intermediateStationConfigurations = intermediateStationConfigurations;
		}

		#endregion

		#region Fields/Constants

		private readonly ISolderConfigurationCollection<UnknownNinnelConfiguration> intermediateStationConfigurations;
		private UnknownNinnelConfiguration inletStationConfiguration;
		private bool? isEnabled;
		private UnknownNinnelConfiguration outletStationConfiguration;
		private string pipelineAssemblyQualifiedTypeName;
		private RecordConfiguration recordConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public ISolderConfigurationCollection<UnknownNinnelConfiguration> IntermediateStationConfigurations
		{
			get
			{
				return this.intermediateStationConfigurations;
			}
		}

		public UnknownNinnelConfiguration InletStationConfiguration
		{
			get
			{
				return this.inletStationConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.inletStationConfiguration, value);
				this.inletStationConfiguration = value;
			}
		}

		public bool? IsEnabled
		{
			get
			{
				return this.isEnabled;
			}
			set
			{
				this.isEnabled = value;
			}
		}

		public UnknownNinnelConfiguration OutletStationConfiguration
		{
			get
			{
				return this.outletStationConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.outletStationConfiguration, value);
				this.outletStationConfiguration = value;
			}
		}

		public string PipelineAssemblyQualifiedTypeName
		{
			get
			{
				return this.pipelineAssemblyQualifiedTypeName;
			}
			set
			{
				this.pipelineAssemblyQualifiedTypeName = value;
			}
		}

		public RecordConfiguration RecordConfiguration
		{
			get
			{
				return this.recordConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.recordConfiguration, value);
				this.recordConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			Type ninnelPipelineType;

			if (string.IsNullOrWhiteSpace(this.PipelineAssemblyQualifiedTypeName))
				yield return new Message(string.Empty, string.Format("{0} assembly qualified type name is required.", context), Severity.Error);
			else
			{
				ninnelPipelineType = this.GetPipelineType();

				if ((object)ninnelPipelineType == null)
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

			if ((object)this.InletStationConfiguration == null)
				yield return new Message(string.Empty, string.Format("Inlet station configuration is required."), Severity.Error);
			else
			{
				var childMessages = this.InletStationConfiguration.Validate(string.Format("{0}/{1}", context, "Inlet station"));

				foreach (var childMessage in childMessages)
				{
					yield return childMessage;
				}
			}

			if ((object)this.IntermediateStationConfigurations == null)
				yield return new Message(string.Empty, string.Format("Intermediate station configurations are required."), Severity.Error);
			else
			{
				var childMessages = this.IntermediateStationConfigurations.Validate(string.Format("{0}/{1}", context, "Intermediate station"));

				foreach (var childMessage in childMessages)
				{
					yield return childMessage;
				}
			}

			if ((object)this.OutletStationConfiguration == null)
				yield return new Message(string.Empty, string.Format("Outlet station configuration is required."), Severity.Error);
			else
			{
				var childMessages = this.OutletStationConfiguration.Validate(string.Format("{0}/{1}", context, "Outlet station"));

				foreach (var childMessage in childMessages)
				{
					yield return childMessage;
				}
			}
		}

		protected override IAsyncEnumerable<IMessage> CoreValidateAsync(object context, CancellationToken cancellationToken = default)
		{
			return null;
		}

		public Type GetPipelineType()
		{
			return ReflectionExtensions.GetTypeFromAssemblyQualifiedTypeName(this.PipelineAssemblyQualifiedTypeName);
		}

		#endregion
	}
}