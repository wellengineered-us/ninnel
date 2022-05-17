/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Configuration
{
	public partial class RecordConfiguration
		: NinnelConfiguration
	{
		#region Methods/Operators

		protected override async IAsyncEnumerable<IMessage> CoreValidateAsync(object context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			if ((object)this.FieldConfigurations == null)
				yield return new Message(string.Empty, string.Format("Field configurations are required."), Severity.Error);
			else
			{
				var childMessages = this.FieldConfigurations.ValidateAsync(string.Format("{0}/{1}", context, "Field"), cancellationToken);

				await foreach (var childMessage in childMessages.WithCancellation(cancellationToken))
				{
					yield return childMessage;
				}

				var fieldNameSums = this.FieldConfigurations
					.GroupBy(c => c.FieldName)
					.Select(cl => new
								{
									ColumnName = cl.First().FieldName,
									Count = cl.Count()
								}).Where(cl => cl.Count > 1);

				foreach (var fieldNameSum in fieldNameSums)
				{
					yield return new Message(string.Empty, string.Format("{0} has a duplicate field name defined: '{1}'.", context, fieldNameSum.ColumnName), Severity.Error);
				}
			}
		}

		#endregion
	}
}
#endif