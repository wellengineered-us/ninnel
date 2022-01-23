/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Middleware
{
	public sealed partial class NinnelMiddlewareClosure<TData, TComponent>
		where TComponent
		: INinnelComponent0
	{
		#region Constructors/Destructors

		private NinnelMiddlewareClosure(NinnelMiddlewareToNextDelegate<TData, TComponent> processToNext, NinnelMiddlewareDelegate<TData, TComponent> next)
		{
			if ((object)processToNext == null)
				throw new ArgumentNullException(nameof(processToNext));

			if ((object)next == null)
				throw new ArgumentNullException(nameof(next));

			this.processToNext = processToNext;
			this.next = next;
		}

		#endregion

		#region Fields/Constants

		private readonly NinnelMiddlewareDelegate<TData, TComponent> next;
		private readonly NinnelMiddlewareToNextDelegate<TData, TComponent> processToNext;

		#endregion

		#region Properties/Indexers/Events

		private NinnelMiddlewareDelegate<TData, TComponent> Next
		{
			get
			{
				return this.next;
			}
		}

		private NinnelMiddlewareToNextDelegate<TData, TComponent> ProcessToNext
		{
			get
			{
				return this.processToNext;
			}
		}

		#endregion

		#region Methods/Operators

		public static NinnelMiddlewareDelegate<TData, TComponent> GetNinnelMiddlewareChain(NinnelMiddlewareToNextDelegate<TData, TComponent> processToNext, NinnelMiddlewareDelegate<TData, TComponent> next)
		{
			if ((object)processToNext == null)
				throw new ArgumentNullException(nameof(processToNext));

			if ((object)next == null)
				throw new ArgumentNullException(nameof(next));

			return new NinnelMiddlewareClosure<TData, TComponent>(processToNext, next).Transform;
		}

		private TComponent Transform(TData data, TComponent target)
		{
			Console.WriteLine("voo doo!");
			return this.ProcessToNext(data, target, this.Next);
		}

		#endregion
	}
}