/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Configuration
{
	public partial class FieldConfiguration
		: NinnelConfiguration
	{
		#region Constructors/Destructors

		public FieldConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string fieldName;

		#endregion

		#region Properties/Indexers/Events

		public string FieldName
		{
			get
			{
				return this.fieldName;
			}
			set
			{
				this.fieldName = value;
			}
		}

		[SolderConfigurationIgnore]
		public new RecordConfiguration Parent
		{
			get
			{
				return (RecordConfiguration)base.Parent;
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
			if (string.IsNullOrWhiteSpace(this.FieldName))
				yield return new Message(string.Empty, string.Format("{0} name is required.", context), Severity.Error);
		}

		public override bool Equals(object obj)
		{
			FieldConfiguration other;

			other = obj as FieldConfiguration;

			if ((object)other != null)
				return other.FieldName.ToStringEx().ToLower() == this.FieldName.ToStringEx().ToLower();

			return false;
		}

		public override int GetHashCode()
		{
			return this.FieldName.ToStringEx().ToLower().GetHashCode();
		}

		#endregion
	}
}