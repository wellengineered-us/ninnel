/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using WellEngineered.Siobhan.Textual.Delimited;

namespace WellEngineered.Ninnel.Station.Textual
{
	public partial class DelimitedTextualSpecification
		: TextualSpecification<DelimitedTextualFieldConfiguration, DelimitedTextualConfiguration, IDelimitedTextualFieldSpec, IDelimitedTextualSpec>
	{
		#region Constructors/Destructors

		public DelimitedTextualSpecification()
		{
		}

		#endregion
	}
}