/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Siobhan.Textual;
using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Station.Textual
{
	public abstract partial class TextualConfiguration<TTextualFieldConfiguration, TTextualFieldSpec, TTextualSpec>
		: NinnelConfiguration
		where TTextualFieldConfiguration : TextualFieldConfiguration
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
	{
		#region Constructors/Destructors

		protected TextualConfiguration(SolderConfigurationCollection<TTextualFieldConfiguration> textualHeaderFieldConfigurations, SolderConfigurationCollection<TTextualFieldConfiguration> textualFooterFieldConfigurations)
		{
			if ((object)textualHeaderFieldConfigurations == null)
				throw new ArgumentNullException(nameof(textualHeaderFieldConfigurations));

			if ((object)textualFooterFieldConfigurations == null)
				throw new ArgumentNullException(nameof(textualFooterFieldConfigurations));

			this.textualHeaderFieldConfigurations = textualHeaderFieldConfigurations;
			this.textualFooterFieldConfigurations = textualFooterFieldConfigurations;
		}

		protected TextualConfiguration()
		{
			this.textualHeaderFieldConfigurations = new SolderConfigurationCollection<TTextualFieldConfiguration>(this);
			this.textualFooterFieldConfigurations = new SolderConfigurationCollection<TTextualFieldConfiguration>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly SolderConfigurationCollection<TTextualFieldConfiguration> textualFooterFieldConfigurations;
		private readonly SolderConfigurationCollection<TTextualFieldConfiguration> textualHeaderFieldConfigurations;
		private bool? firstRecordIsHeader;
		private bool? lastRecordIsFooter;
		private string recordDelimiter;

		#endregion

		#region Properties/Indexers/Events

		public SolderConfigurationCollection<TTextualFieldConfiguration> TextualFooterFieldConfigurations
		{
			get
			{
				return this.textualFooterFieldConfigurations;
			}
		}

		public SolderConfigurationCollection<TTextualFieldConfiguration> TextualHeaderFieldConfigurations
		{
			get
			{
				return this.textualHeaderFieldConfigurations;
			}
		}

		public bool? FirstRecordIsHeader
		{
			get
			{
				return this.firstRecordIsHeader;
			}
			set
			{
				this.firstRecordIsHeader = value;
			}
		}

		public bool? LastRecordIsFooter
		{
			get
			{
				return this.lastRecordIsFooter;
			}
			set
			{
				this.lastRecordIsFooter = value;
			}
		}

		public string RecordDelimiter
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

		#endregion

		#region Methods/Operators

		public abstract TTextualSpec MapToSpec();

		#endregion
	}
}