/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

using WellEngineered.Siobhan.Textual.Delimited;

namespace WellEngineered.Ninnel.Station.Textual
{
	public partial class DelimitedTextInletStation
		: TextualInletStation
		<DelimitedTextualFieldConfiguration, DelimitedTextualConfiguration,
			IDelimitedTextualFieldSpec, IDelimitedTextualSpec,
			DelimitedTextualSpecification, DelimitedTextualReader>
	{
		#region Constructors/Destructors

		public DelimitedTextInletStation()
		{
		}

		#endregion

		#region Methods/Operators

		protected override DelimitedTextualReader CoreCreateTextualReader(StreamReader streamReader, IDelimitedTextualSpec textualSpec)
		{
			if ((object)streamReader == null)
				throw new ArgumentNullException(nameof(streamReader));

			if ((object)textualSpec == null)
				throw new ArgumentNullException(nameof(textualSpec));

			return new DelimitedTextualReader(streamReader, textualSpec);
		}

		#endregion
	}
}