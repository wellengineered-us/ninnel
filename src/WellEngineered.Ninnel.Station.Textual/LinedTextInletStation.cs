/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

using WellEngineered.Siobhan.Textual.Lined;

namespace WellEngineered.Ninnel.Station.Textual
{
	public partial class LinedTextInletStation
		: TextualInletStation
		<LinedTextualFieldConfiguration, LinedTextualConfiguration,
			ILinedTextualFieldSpec, ILinedTextualSpec,
			LinedTextualSpecification, LinedTextualReader>
	{
		#region Constructors/Destructors

		public LinedTextInletStation()
		{
		}

		#endregion

		#region Methods/Operators

		protected override LinedTextualReader CoreCreateTextualReader(StreamReader streamReader, ILinedTextualSpec textualSpec)
		{
			if ((object)streamReader == null)
				throw new ArgumentNullException(nameof(streamReader));

			if ((object)textualSpec == null)
				throw new ArgumentNullException(nameof(textualSpec));

			return new LinedTextualReader(streamReader, textualSpec);
		}

		#endregion
	}
}