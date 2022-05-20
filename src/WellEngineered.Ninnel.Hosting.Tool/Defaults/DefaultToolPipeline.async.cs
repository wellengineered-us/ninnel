/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Context;
using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Transport;
using WellEngineered.Solder.Injection;

namespace WellEngineered.Ninnel.Hosting.Tool.Defaults
{
	public partial class DefaultToolPipeline
		: NinnelPipeline
	{
		#region Methods/Operators

		protected override async ValueTask<INinnelContext> CoreCloneContextAsync(INinnelContext ninnelContext, CancellationToken cancellationToken = default)
		{
			await Task.CompletedTask;

			// for now, just return this one
			return ninnelContext;
		}

		protected override async ValueTask<INinnelContext> CoreCreateContextAsync(Type ninnelContextType, CancellationToken cancellationToken = default)
		{
			if ((object)ninnelContextType == null)
				throw new ArgumentNullException(nameof(ninnelContextType));

			// TODO - Fix this
			return await new DefaultComponentFactory().CreateNinnelComponentAsync<INinnelContext>(AssemblyDomain.Default.DependencyManager, ninnelContextType, (this.Configuration.Parent.ComponentAutoWire ?? true), null, true, cancellationToken);
		}

		protected override async ValueTask<long> CoreExecuteAsync(INinnelContext ninnelContext, CancellationToken cancellationToken = default)
		{
			await AssemblyDomain.Default.ResourceManager.PrintAsync(Guid.Empty, "Mark", cancellationToken);
			await Task.Delay(4000, cancellationToken);
			await AssemblyDomain.Default.ResourceManager.PrintAsync(Guid.Empty, "Set", cancellationToken);
			await Task.Delay(2000, cancellationToken);
			await AssemblyDomain.Default.ResourceManager.PrintAsync(Guid.Empty, "Ready", cancellationToken);
			await Task.Delay(1000, cancellationToken);
			await AssemblyDomain.Default.ResourceManager.PrintAsync(Guid.Empty, "Go", cancellationToken);

			return 0;
		}

		#endregion
	}
}
#endif