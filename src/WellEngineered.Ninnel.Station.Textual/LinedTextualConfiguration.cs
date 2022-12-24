/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Siobhan.Textual.Lined;

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

		#region Fields/Constants

		private NewLineStyle? newLineStyle;

		#endregion

		#region Properties/Indexers/Events

		public NewLineStyle? NewLineStyle
		{
			get
			{
				return this.newLineStyle;
			}
			set
			{
				this.newLineStyle = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override ILinedTextualSpec MapToSpec()
		{
			LinedTextualSpec linedTextualSpec;

			linedTextualSpec = new LinedTextualSpec()
								{
									ContentEncoding = this.ContentEncoding,
									NewLineStyle = this.NewLineStyle ?? Siobhan.Textual.Lined.NewLineStyle.Auto
								};

			return linedTextualSpec;
		}

		#endregion
	}
}