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

namespace WellEngineered.Ninnel.Hosting.Tool.Defaults
{
	public sealed partial class _DemoSpecification
		: NinnelSpecification
	{
		#region Methods/Operators
		
		protected override async IAsyncEnumerable<IMessage> CoreValidateAsync(object context, [EnumeratorCancellation] CancellationToken cancellationToken = new CancellationToken())
		{
			if (string.IsNullOrWhiteSpace(this._))
				yield return new Message(String.Empty, string.Format("Specification requires property '{0}' to be set to any non-whitespace value.", nameof(this.__)), Severity.Error);

			await Task.CompletedTask;
		}

		#endregion
	}
}
#endif