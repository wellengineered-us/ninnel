/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Siobhan.Textual;
using WellEngineered.Siobhan.Textual.Delimited;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Station.Textual
{
	public partial class DelimitedTextualConfiguration
		: TextualConfiguration<DelimitedTextualFieldConfiguration, IDelimitedTextualFieldSpec, IDelimitedTextualSpec>
	{
		#region Constructors/Destructors

		public DelimitedTextualConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string closeQuoteValue;
		private string fieldDelimiter;
		private string openQuoteValue;

		#endregion

		#region Properties/Indexers/Events

		public string CloseQuoteValue
		{
			get
			{
				return this.closeQuoteValue;
			}
			set
			{
				this.closeQuoteValue = value;
			}
		}

		public string FieldDelimiter
		{
			get
			{
				return this.fieldDelimiter;
			}
			set
			{
				this.fieldDelimiter = value;
			}
		}

		public string OpenQuoteValue
		{
			get
			{
				return this.openQuoteValue;
			}
			set
			{
				this.openQuoteValue = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IDelimitedTextualSpec MapToSpec()
		{
			DelimitedTextualSpec delimitedTextualSpec;

			delimitedTextualSpec = new DelimitedTextualSpec()
									{
										CloseQuoteValue = this.CloseQuoteValue,
										FieldDelimiter = this.FieldDelimiter,
										IsFirstRecordHeader = this.FirstRecordIsHeader ?? false,
										IsLastRecordFooter = this.LastRecordIsFooter ?? false,
										OpenQuoteValue = this.OpenQuoteValue,
										RecordDelimiter = this.RecordDelimiter
									};

			foreach (DelimitedTextualFieldConfiguration delimitedTextHeaderFieldConfiguration in this.TextualHeaderFieldConfigurations)
			{
				DelimitedTextualFieldSpec delimitedTextualFieldSpec;

				delimitedTextualFieldSpec = new DelimitedTextualFieldSpec()
											{
												FieldTitle = delimitedTextHeaderFieldConfiguration.FieldTitle,
												FieldFormat = delimitedTextHeaderFieldConfiguration.FieldFormat,
												IsFieldIdentity = delimitedTextHeaderFieldConfiguration.IsFieldIdentity ?? false,
												FieldType = delimitedTextHeaderFieldConfiguration.FieldType ?? TextualFieldType.Text,
												IsFieldRequired = delimitedTextHeaderFieldConfiguration.IsFieldRequired ?? false,
												FieldOrdinal = delimitedTextHeaderFieldConfiguration.FieldOrdinal ?? delimitedTextualSpec.TextualHeaderSpecs.Count
											};

				delimitedTextualSpec.TextualHeaderSpecs.Add(delimitedTextualFieldSpec);
			}

			return delimitedTextualSpec;
		}

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			var childMessages =  base.CoreValidate(context);

			foreach (var childMessage in childMessages)
			{
				yield return childMessage;
			}
			
			if (string.IsNullOrEmpty(this.RecordDelimiter))
				yield return new Message(string.Empty, string.Format("{0} textual (delimited) record delimiter is required.", context), Severity.Error);
             
             if (string.IsNullOrEmpty(this.FieldDelimiter))
				yield return new Message(string.Empty, string.Format("{0} textual (delimited) field delimiter is required.", context), Severity.Error);
		}

		#endregion
	}
}