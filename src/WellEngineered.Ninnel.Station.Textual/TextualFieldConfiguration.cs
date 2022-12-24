/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Siobhan.Textual;

namespace WellEngineered.Ninnel.Station.Textual
{
	public abstract partial class TextualFieldConfiguration
		: NinnelConfiguration
	{
		#region Constructors/Destructors

		protected TextualFieldConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string fieldFormat;
		private long? fieldOrdinal;
		private string fieldTitle;
		private TextualFieldType? fieldType;
		private bool? isFieldIdentity;
		private bool? isFieldRequired;

		#endregion

		#region Properties/Indexers/Events

		public string FieldFormat
		{
			get
			{
				return this.fieldFormat;
			}
			set
			{
				this.fieldFormat = value;
			}
		}

		public long? FieldOrdinal
		{
			get
			{
				return this.fieldOrdinal;
			}
			set
			{
				this.fieldOrdinal = value;
			}
		}

		public string FieldTitle
		{
			get
			{
				return this.fieldTitle;
			}
			set
			{
				this.fieldTitle = value;
			}
		}

		public TextualFieldType? FieldType
		{
			get
			{
				return this.fieldType;
			}
			set
			{
				this.fieldType = value;
			}
		}

		public bool? IsFieldIdentity
		{
			get
			{
				return this.isFieldIdentity;
			}
			set
			{
				this.isFieldIdentity = value;
			}
		}

		public bool? IsFieldRequired
		{
			get
			{
				return this.isFieldRequired;
			}
			set
			{
				this.isFieldRequired = value;
			}
		}

		#endregion
	}
}