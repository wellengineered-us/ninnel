/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Middleware
{
	public sealed class AsyncNinnelMiddlewareClosure<TData, TComponent>
		where TComponent : IAsyncLifecycle
	{
		#region Constructors/Destructors

		private AsyncNinnelMiddlewareClosure(AsyncNinnelMiddlewareToNextDelegate<TData, TComponent> asyncProcessToNext, AsyncNinnelMiddlewareDelegate<TData, TComponent> asyncNext, CancellationToken cancellationToken = default)
		{
			if ((object)asyncProcessToNext == null)
				throw new ArgumentNullException(nameof(asyncProcessToNext));

			if ((object)asyncNext == null)
				throw new ArgumentNullException(nameof(asyncNext));

			this.asyncProcessToNext = asyncProcessToNext;
			this.asyncNext = asyncNext;
			this.cancellationToken = cancellationToken;
		}

		#endregion

		#region Fields/Constants

		private readonly AsyncNinnelMiddlewareDelegate<TData, TComponent> asyncNext;
		private readonly AsyncNinnelMiddlewareToNextDelegate<TData, TComponent> asyncProcessToNext;
		private readonly CancellationToken cancellationToken;

		#endregion

		#region Properties/Indexers/Events

		private AsyncNinnelMiddlewareDelegate<TData, TComponent> AsyncNext
		{
			get
			{
				return this.asyncNext;
			}
		}

		private AsyncNinnelMiddlewareToNextDelegate<TData, TComponent> AsyncProcessToNext
		{
			get
			{
				return this.asyncProcessToNext;
			}
		}

		private CancellationToken CancelationToken
		{
			get
			{
				return this.cancellationToken;
			}
		}

		#endregion

		#region Methods/Operators

		public static AsyncNinnelMiddlewareDelegate<TData, TComponent> GetNinnelMiddlewareChain(AsyncNinnelMiddlewareToNextDelegate<TData, TComponent> asyncProcessToNext, AsyncNinnelMiddlewareDelegate<TData, TComponent> asyncNext, CancellationToken cancellationToken = default)
		{
			if ((object)asyncProcessToNext == null)
				throw new ArgumentNullException(nameof(asyncProcessToNext));

			if ((object)asyncNext == null)
				throw new ArgumentNullException(nameof(asyncNext));

			return new AsyncNinnelMiddlewareClosure<TData, TComponent>(asyncProcessToNext, asyncNext, cancellationToken).TransformAsync;
		}

		private async ValueTask<TComponent> TransformAsync(TData data, TComponent target, CancellationToken cancellationToken = default)
		{
			TComponent component;

			await Console.Out.WriteLineAsync("voo doo!");
			component = await this.AsyncProcessToNext(data, target, this.AsyncNext, cancellationToken);
			return component;
		}

		#endregion
	}
}
#endif