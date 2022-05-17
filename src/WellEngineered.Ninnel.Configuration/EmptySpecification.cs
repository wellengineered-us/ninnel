/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Configuration
{
	public sealed partial class EmptySpecification
		: NinnelSpecification
	{
		#region Constructors/Destructors

		public EmptySpecification()
		{
		}

		#endregion

		#region Fields/Constants

		private string __;

		#endregion

		#region Properties/Indexers/Events

		public string _
		{
			get
			{
				return this.__;
			}
			set
			{
				this.__ = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			if (string.IsNullOrWhiteSpace(this._))
				yield return new Message(String.Empty, string.Format("Empty specification requires property '{0}' to be set to any non-whitespace value.", nameof(this.__)), Severity.Error);
		}

		#endregion
	}
}