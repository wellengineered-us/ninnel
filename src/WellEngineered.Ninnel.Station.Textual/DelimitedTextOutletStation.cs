/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

using WellEngineered.Siobhan.Textual.Delimited;

namespace WellEngineered.Ninnel.Station.Textual
{
	public partial class DelimitedTextOutletStation
		: TextualOutletStation
		<DelimitedTextualFieldConfiguration, DelimitedTextualConfiguration,
			IDelimitedTextualFieldSpec, IDelimitedTextualSpec,
			DelimitedTextualSpecification, DelimitedTextualWriter>
	{
		#region Constructors/Destructors

		public DelimitedTextOutletStation()
		{
		}

		#endregion

		#region Methods/Operators

		protected override DelimitedTextualWriter CoreCreateTextualWriter(StreamWriter streamWriter, IDelimitedTextualSpec textualSpec)
		{
			if ((object)streamWriter == null)
				throw new ArgumentNullException(nameof(streamWriter));

			if ((object)textualSpec == null)
				throw new ArgumentNullException(nameof(textualSpec));

			return new DelimitedTextualWriter(streamWriter, textualSpec);
		}

		#endregion
	}
}