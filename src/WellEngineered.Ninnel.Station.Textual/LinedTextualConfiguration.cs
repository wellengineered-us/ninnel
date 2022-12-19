/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Siobhan.Textual;
using WellEngineered.Siobhan.Textual.Lined;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Station.Textual
{
	public partial class LinedTextualConfiguration
		: TextualConfiguration<LinedTextualFieldConfiguration, ILinedTextualFieldSpec, ILinedTextualSpec>
	{
		#region Constructors/Destructors

		public LinedTextualConfiguration()
		{
		}

		#endregion

		#region Methods/Operators

		public override ILinedTextualSpec MapToSpec()
		{
			LinedTextualSpec linedTextualSpec;

			linedTextualSpec = new LinedTextualSpec()
								{
									IsFirstRecordHeader = this.FirstRecordIsHeader ?? false,
									IsLastRecordFooter = this.LastRecordIsFooter ?? false,
									RecordDelimiter = this.RecordDelimiter
								};

			foreach (LinedTextualFieldConfiguration linedTextHeaderFieldConfiguration in this.TextualHeaderFieldConfigurations)
			{
				LinedTextualFieldSpec linedTextualFieldSpec;

				linedTextualFieldSpec = new LinedTextualFieldSpec()
										{
											FieldTitle = linedTextHeaderFieldConfiguration.FieldTitle,
											FieldFormat = linedTextHeaderFieldConfiguration.FieldFormat,
											IsFieldIdentity = linedTextHeaderFieldConfiguration.IsFieldIdentity ?? false,
											FieldType = linedTextHeaderFieldConfiguration.FieldType ?? TextualFieldType.Text,
											IsFieldRequired = linedTextHeaderFieldConfiguration.IsFieldRequired ?? false,
											FieldOrdinal = linedTextHeaderFieldConfiguration.FieldOrdinal ?? linedTextualSpec.TextualHeaderSpecs.Count
										};

				linedTextualSpec.TextualHeaderSpecs.Add(linedTextualFieldSpec);
			}

			return linedTextualSpec;
		}

		#endregion
	}
}