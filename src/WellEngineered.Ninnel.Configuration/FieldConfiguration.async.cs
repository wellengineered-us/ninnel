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

using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Configuration
{
	public partial class FieldConfiguration
		: NinnelConfiguration
	{
		#region Methods/Operators

		protected override async IAsyncEnumerable<IMessage> CoreValidateAsync(object context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(this.FieldName))
				yield return new Message(string.Empty, string.Format("{0} name is required.", context), Severity.Error);

			await Task.CompletedTask;
		}

		#endregion
	}
}
#endif