/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Primitives.Configuration
{
	public abstract partial class NinnelConfiguration
		: SolderConfiguration,
			INinnelConfiguration
	{
		#region Methods/Operators

		protected async override IAsyncEnumerable<IMessage> CoreValidateAsync(object context, [EnumeratorCancellation] CancellationToken cancellationToken = new CancellationToken())
		{
			await Task.CompletedTask;
			yield break;
		}

		#endregion
	}
}
#endif