/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Siobhan.Textual;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Station.Textual
{
	public abstract partial class TextualSpecification<TTextualFieldConfiguration, TTextualConfiguration, TTextualFieldSpec, TTextualSpec>
		: NinnelSpecification
		where TTextualFieldConfiguration : TextualFieldConfiguration
		where TTextualConfiguration : TextualConfiguration<TTextualFieldConfiguration, TTextualFieldSpec, TTextualSpec>
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
	{
		#region Constructors/Destructors

		protected TextualSpecification()
		{
		}

		#endregion

		#region Fields/Constants

		private TTextualConfiguration textualConfiguration;

		private string textualFilePath;

		#endregion

		#region Properties/Indexers/Events

		public TTextualConfiguration TextualConfiguration
		{
			get
			{
				return this.textualConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.textualConfiguration, value);
				this.textualConfiguration = value;
			}
		}

		public string TextualFilePath
		{
			get
			{
				return this.textualFilePath;
			}
			set
			{
				this.textualFilePath = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			string stationContext;

			stationContext = context as string;
			
			if (string.IsNullOrWhiteSpace(this.TextualFilePath))
				yield return new Message(string.Empty, string.Format("{0} station textual file path is required.", stationContext), Severity.Error);

			if ((object)this.TextualConfiguration == null)
				yield return new Message(string.Empty, string.Format("{0} station textual specification is required.", stationContext), Severity.Error);
			else
			{
				var childMessages = this.TextualConfiguration.Validate("Station");

				foreach (var childMessage in childMessages)
				{
					yield return childMessage;
				}
			}
		}

		#endregion
	}
}