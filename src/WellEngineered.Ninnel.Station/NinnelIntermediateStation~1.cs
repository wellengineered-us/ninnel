/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Middleware;
using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Station
{
	public abstract partial class NinnelIntermediateStation<TNinnelSpecification>
		: NinnelStation<TNinnelSpecification>,
			INinnelIntermediateStation<TNinnelSpecification>
		where TNinnelSpecification : class, INinnelSpecification, new()
	{
		#region Constructors/Destructors

		protected NinnelIntermediateStation()
		{
		}

		#endregion

		#region Properties/Indexers/Events
		
		protected sealed override void CorePostExecute(NinnelStationFrame ninnelStationFrame)
		{
			throw new NinnelException(string.Format("Post execution semantics are not supported for this component type."));
		}

		protected sealed override void CorePreExecute(NinnelStationFrame ninnelStationFrame)
		{
			throw new NinnelException(string.Format("Pre execution semantics are not supported for this component type."));
		}

		/*INinnelStream INinnelMiddleware<NinnelStationFrame, INinnelStream, IUnknownNinnelConfiguration>.Process(NinnelStationFrame ninnelStationFrame, INinnelStream ninnelStream, NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> next)
		{
			//if ((object)ninnelStationFrame == null)
			//throw new ArgumentNullException(nameof(ninnelStationFrame));

			//if ((object)ninnelStream == null)
			//throw new ArgumentNullException(nameof(ninnelStream));

			//if ((object)next == null)
			//throw new ArgumentNullException(nameof(next));

			try
			{
				return this.CoreProcess(ninnelStationFrame, ninnelStream, next);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The intermediate station failed (see inner exception)."), ex);
			}
		}*/

		IUnknownNinnelConfiguration<TNinnelSpecification> IConfigurable<IUnknownNinnelConfiguration<TNinnelSpecification>>.Configuration
		{
			get
			{
				return base.Configuration;
			}
			set
			{
				base.Configuration = new UnknownNinnelConfiguration<TNinnelSpecification>(value);
			}
		}

		/*public ILifecycle Process(object data, ILifecycle target, NinnelMiddlewareDelegate next)
		{
			return this.CoreProcess((NinnelStationFrame)data, (INinnelStream)target, (frame, stream) => (INinnelStream)next(frame, stream));
		}*/

		/*public async ValueTask<IAsyncLifecycle> ProcessAsync(object data, IAsyncLifecycle target, AsyncNinnelMiddlewareDelegate next, CancellationToken cancellationToken = default)
		{
			var q = this.CoreProcessAsync((NinnelStationFrame)data, (IAsyncNinnelStream)target, async (frame, stream) =>
																								{
																									var x = next(frame, stream);
																									var y = (IAsyncNinnelStream)await x;
																									return y;
																								}, cancellationToken);

			var i = await q;
			return i;
		}*/

		#endregion

		#region Methods/Operators

		protected abstract INinnelStream CoreProcess(NinnelStationFrame ninnelStationFrame, INinnelStream ninnelStream, NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> next);

		INinnelStream INinnelMiddleware<NinnelStationFrame, INinnelStream, IUnknownNinnelConfiguration<TNinnelSpecification>>.Process(NinnelStationFrame ninnelStationFrame, INinnelStream ninnelStream, NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> next)
		{
			//if ((object)ninnelStationFrame == null)
			//throw new ArgumentNullException(nameof(ninnelStationFrame));

			//if ((object)ninnelStream == null)
			//throw new ArgumentNullException(nameof(ninnelStream));

			//if ((object)next == null)
			//throw new ArgumentNullException(nameof(next));

			try
			{
				return this.CoreProcess(ninnelStationFrame, ninnelStream, next);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The intermediate station failed (see inner exception)."), ex);
			}
		}

		#endregion

		/*IUnknownNinnelConfiguration IConfigurable<IUnknownNinnelConfiguration>.Configuration
		{
			get
			{
				return base.Configuration;
			}
			set
			{
				base.Configuration = new UnknownNinnelConfiguration<TNinnelSpecification>(value);
			}
		}*/
	}
}