/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Context;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Transport;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public class DefaultPipeline
		: NinnelPipeline
	{
		#region Constructors/Destructors

		public DefaultPipeline()
		{
		}

		#endregion

		#region Methods/Operators

		protected override INinnelContext CoreCloneContext(INinnelContext ninnelContext)
		{
			return null;
		}

		protected override ValueTask<INinnelContext> CoreCloneContextAsync(INinnelContext ninnelContext, CancellationToken cancellationToken = default)
		{
			return default;
		}

		protected INinnelContext CoreCreateContext(Type ninnelContextType, bool autoWire)
		{
			INinnelContext ninnelContext;

			if ((object)ninnelContextType == null)
				throw new ArgumentNullException(nameof(ninnelContextType));

			if (autoWire)
				ninnelContext = ninnelContextType.ResolveAutoWireAssignableToTargetType<INinnelContext>(AssemblyDomain.Default.DependencyManager);
			else
				ninnelContext = ninnelContextType.CreateInstanceAssignableToTargetType<INinnelContext>();

			if ((object)ninnelContext == null)
				throw new NinnelException(string.Format("Failed to instantiate context type: '{0}', auto-wire: {1}.", ninnelContextType.FullName, autoWire));

			return ninnelContext;
		}

		protected override INinnelContext CoreCreateContext()
		{
			return this.CoreCreateContext(typeof(DefaultToolContext), false);
		}

		protected async override ValueTask<INinnelContext> CoreCreateContextAsync(CancellationToken cancellationToken = default)
		{
			await Task.CompletedTask;
			return new DefaultToolContext();
		}

		protected override long CoreExecute(INinnelContext ninnelContext)
		{
			AssemblyDomain.Default.ResourceManager.Print(Guid.Empty, "Pipeline.Execute(): Mark");
			Thread.Sleep(4000);
			AssemblyDomain.Default.ResourceManager.Print(Guid.Empty, "Pipeline.Execute(): Set");
			Thread.Sleep(2000);
			AssemblyDomain.Default.ResourceManager.Print(Guid.Empty, "Pipeline.Execute(): Ready");
			Thread.Sleep(1000);
			AssemblyDomain.Default.ResourceManager.Print(Guid.Empty, "Pipeline.Execute(): Go");

			return 0;
		}

		protected override ValueTask<long> CoreExecuteAsync(INinnelContext ninnelContext, CancellationToken cancellationToken = default)
		{
			return default;
		}

		#endregion
	}
}