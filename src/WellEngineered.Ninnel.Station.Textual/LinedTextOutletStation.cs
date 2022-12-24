/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

using WellEngineered.Siobhan.Textual.Lined;

namespace WellEngineered.Ninnel.Station.Textual
{
	public partial class LinedTextOutletStation
		: TextualOutletStation
		<LinedTextualFieldConfiguration, LinedTextualConfiguration,
			ILinedTextualFieldSpec, ILinedTextualSpec,
			LinedTextualSpecification, LinedTextualWriter>
	{
		#region Constructors/Destructors

		public LinedTextOutletStation()
		{
		}

		#endregion

		#region Methods/Operators

		protected override LinedTextualWriter CoreCreateTextualWriter(StreamWriter streamWriter, ILinedTextualSpec textualSpec)
		{
			if ((object)streamWriter == null)
				throw new ArgumentNullException(nameof(streamWriter));

			if ((object)textualSpec == null)
				throw new ArgumentNullException(nameof(textualSpec));

			return new LinedTextualWriter(streamWriter, textualSpec);
		}

		#endregion
	}
}