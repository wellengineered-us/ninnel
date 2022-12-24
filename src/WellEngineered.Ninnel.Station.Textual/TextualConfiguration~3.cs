/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Siobhan.Textual;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Station.Textual
{
	public abstract partial class TextualConfiguration<TTextualFieldConfiguration, TTextualFieldSpec, TTextualSpec>
		: NinnelConfiguration
		where TTextualFieldConfiguration : TextualFieldConfiguration
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
	{
		#region Constructors/Destructors

		protected TextualConfiguration(SolderConfigurationCollection<TTextualFieldConfiguration> textualHeaderFieldConfigurations)
		{
			if ((object)textualHeaderFieldConfigurations == null)
				throw new ArgumentNullException(nameof(textualHeaderFieldConfigurations));

			this.textualHeaderFieldConfigurations = textualHeaderFieldConfigurations;
		}

		protected TextualConfiguration()
		{
			this.textualHeaderFieldConfigurations = new SolderConfigurationCollection<TTextualFieldConfiguration>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly SolderConfigurationCollection<TTextualFieldConfiguration> textualHeaderFieldConfigurations;
		private string contentEncoding;
		
		#endregion

		#region Properties/Indexers/Events

		public SolderConfigurationCollection<TTextualFieldConfiguration> TextualHeaderFieldConfigurations
		{
			get
			{
				return this.textualHeaderFieldConfigurations;
			}
		}

		public string ContentEncoding
		{
			get
			{
				return this.contentEncoding;
			}
			set
			{
				this.contentEncoding = value;
			}
		}

		#endregion

		#region Methods/Operators

		public abstract TTextualSpec MapToSpec();

		#endregion
	}
}