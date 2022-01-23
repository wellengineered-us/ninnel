/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Configuration
{
	public class RecordConfiguration : NinnelConfiguration
	{
		#region Constructors/Destructors

		public RecordConfiguration()
		{
			this.fieldConfigurations = new SolderConfigurationCollection<FieldConfiguration>(this);
		}

		public RecordConfiguration(ISolderConfigurationCollection<FieldConfiguration> fieldConfigurations)
		{
			if ((object)fieldConfigurations == null)
				throw new ArgumentNullException(nameof(fieldConfigurations));

			this.fieldConfigurations = fieldConfigurations;
		}

		#endregion

		#region Fields/Constants

		private readonly ISolderConfigurationCollection<FieldConfiguration> fieldConfigurations;

		#endregion

		#region Properties/Indexers/Events

		public ISolderConfigurationCollection<FieldConfiguration> FieldConfigurations
		{
			get
			{
				return this.fieldConfigurations;
			}
		}

		[SolderConfigurationIgnore]
		public new PipelineConfiguration Parent
		{
			get
			{
				return (PipelineConfiguration)base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			if ((object)this.FieldConfigurations == null)
				yield return new Message(string.Empty, string.Format("Field configurations are required."), Severity.Error);
			else
			{
				var childMessages = this.FieldConfigurations.Validate(string.Format("{0}/{1}", context, "Field"));

				foreach (var childMessage in childMessages)
				{
					yield return childMessage;
				}

				var fieldNameSums = this.FieldConfigurations
					.GroupBy(c => c.FieldName)
					.Select(cl => new
								{
									ColumnName = cl.First().FieldName,
									Count = cl.Count()
								}).Where(cl => cl.Count > 1);

				foreach (var fieldNameSum in fieldNameSums)
				{
					yield return new Message(string.Empty, string.Format("{0} has a duplicate field name defined: '{1}'.", context, fieldNameSum.ColumnName), Severity.Error);
				}
			}
		}

		protected override IAsyncEnumerable<IMessage> CoreValidateAsync(object context, CancellationToken cancellationToken = default)
		{
			return null;
		}

		#endregion
	}
}