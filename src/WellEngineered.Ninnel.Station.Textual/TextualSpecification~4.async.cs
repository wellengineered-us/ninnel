/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

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
		#region Methods/Operators

		protected async override IAsyncEnumerable<IMessage> CoreValidateAsync(object context, [EnumeratorCancellation] CancellationToken cancellationToken = new CancellationToken())
		{
			string stationContext;

			stationContext = context as string;

			if (string.IsNullOrWhiteSpace(this.TextualFilePath))
				yield return new Message(string.Empty, string.Format("{0} station textual file path is required.", stationContext), Severity.Error);

			if ((object)this.TextualConfiguration == null)
				yield return new Message(string.Empty, string.Format("{0} station textual specification is required.", stationContext), Severity.Error);
			else
			{
				var childMessages = this.TextualConfiguration.ValidateAsync("Station", cancellationToken);

				await foreach (var childMessage in childMessages.WithCancellation(cancellationToken))
				{
					yield return childMessage;
				}
			}
		}

		#endregion
	}
}
#endif