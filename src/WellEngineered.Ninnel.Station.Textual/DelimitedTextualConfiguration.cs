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
		private bool? isFirstRecordHeader;
		private string recordDelimiter;
		private int? skipInitialRecordCount;

		#endregion

		#region Properties/Indexers/Events
		
		public int? SkipInitialRecordCount
		{
			get
			{
				return this.skipInitialRecordCount;
			}
			set
			{
				this.skipInitialRecordCount = value;
			}
		}

		public bool? IsFirstRecordHeader
		{
			get
			{
				return this.isFirstRecordHeader;
			}
			set
			{
				this.isFirstRecordHeader = value;
			}
		}

		public virtual string RecordDelimiter
		{
			get
			{
				return this.recordDelimiter;
			}
			set
			{
				this.recordDelimiter = value;
			}
		}
		
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

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			var childMessages = base.CoreValidate(context);

			foreach (var childMessage in childMessages)
			{
				yield return childMessage;
			}

			if (string.IsNullOrEmpty(this.RecordDelimiter))
				yield return new Message(string.Empty, string.Format("{0} textual (delimited) record delimiter is required.", context), Severity.Error);

			if (string.IsNullOrEmpty(this.FieldDelimiter))
				yield return new Message(string.Empty, string.Format("{0} textual (delimited) field delimiter is required.", context), Severity.Error);
		}

		public override IDelimitedTextualSpec MapToSpec()
		{
			DelimitedTextualSpec delimitedTextualSpec;

			delimitedTextualSpec = new DelimitedTextualSpec()
									{
										ContentEncoding = this.ContentEncoding,
										
										IsFirstRecordHeader = this.IsFirstRecordHeader ?? false,
										RecordDelimiter = this.RecordDelimiter,
										FieldDelimiter = this.FieldDelimiter,
										CloseQuoteValue = this.CloseQuoteValue,
										OpenQuoteValue = this.OpenQuoteValue
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
												FieldOrdinal = delimitedTextHeaderFieldConfiguration.FieldOrdinal ?? delimitedTextualSpec.HeaderSpecs.Count
											};

				delimitedTextualSpec.HeaderSpecs.Add(delimitedTextualFieldSpec);
			}

			return delimitedTextualSpec;
		}

		#endregion
	}
}