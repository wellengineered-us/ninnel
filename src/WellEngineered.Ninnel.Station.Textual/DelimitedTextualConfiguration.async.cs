/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Siobhan.Textual.Delimited;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Station.Textual
{
	public partial class DelimitedTextualConfiguration
		: TextualConfiguration<DelimitedTextualFieldConfiguration, IDelimitedTextualFieldSpec, IDelimitedTextualSpec>
	{
		#region Methods/Operators

		protected async override IAsyncEnumerable<IMessage> CoreValidateAsync(object context, [EnumeratorCancellation] CancellationToken cancellationToken = new CancellationToken())
		{
			var childMessages = base.CoreValidateAsync(context, cancellationToken);

			await foreach (var childMessage in childMessages.WithCancellation(cancellationToken))
			{
				yield return childMessage;
			}

			if (string.IsNullOrEmpty(this.RecordDelimiter))
				yield return new Message(string.Empty, string.Format("{0} textual (delimited) record delimiter is required.", context), Severity.Error);

			if (string.IsNullOrEmpty(this.FieldDelimiter))
				yield return new Message(string.Empty, string.Format("{0} textual (delimited) field delimiter is required.", context), Severity.Error);
		}

		#endregion
	}
}
#endif