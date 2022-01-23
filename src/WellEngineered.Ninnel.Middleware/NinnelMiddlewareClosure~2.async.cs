/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Middleware
{
	public sealed partial class NinnelMiddlewareClosure<TData, TComponent>
		where TComponent
		: INinnelComponent0
	{
		#region Constructors/Destructors

		private NinnelMiddlewareClosure(AsyncNinnelMiddlewareToNextDelegate<TData, TComponent> asyncProcessToNext, AsyncNinnelMiddlewareDelegate<TData, TComponent> asyncNext)
		{
			if ((object)asyncProcessToNext == null)
				throw new ArgumentNullException(nameof(asyncProcessToNext));

			if ((object)asyncNext == null)
				throw new ArgumentNullException(nameof(asyncNext));

			this.asyncProcessToNext = asyncProcessToNext;
			this.asyncNext = asyncNext;
		}

		#endregion

		#region Fields/Constants

		private readonly AsyncNinnelMiddlewareDelegate<TData, TComponent> asyncNext;
		private readonly AsyncNinnelMiddlewareToNextDelegate<TData, TComponent> asyncProcessToNext;

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

		#endregion

		#region Methods/Operators

		public static AsyncNinnelMiddlewareDelegate<TData, TComponent> GetNinnelMiddlewareChain(AsyncNinnelMiddlewareToNextDelegate<TData, TComponent> asyncProcessToNext, AsyncNinnelMiddlewareDelegate<TData, TComponent> asyncNext)
		{
			if ((object)asyncProcessToNext == null)
				throw new ArgumentNullException(nameof(asyncProcessToNext));

			if ((object)asyncNext == null)
				throw new ArgumentNullException(nameof(asyncNext));

			return new NinnelMiddlewareClosure<TData, TComponent>(asyncProcessToNext, asyncNext).TransformAsync;
		}

		private async ValueTask<TComponent> TransformAsync(TData data, TComponent target)
		{
			TComponent component;

			await Console.Out.WriteLineAsync("voo doo!");
			component = await this.AsyncProcessToNext(data, target, this.AsyncNext);
			return component;
		}

		#endregion
	}
}