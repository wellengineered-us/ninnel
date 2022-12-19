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
using WellEngineered.Ninnel.Station;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Hosting.Tool.Defaults
{
	public partial class _DemoIntermediateStation : NinnelIntermediateStation<_DemoSpecification>
	{
		public _DemoIntermediateStation()
		{
			this._ = "default";
		}
		
		public _DemoIntermediateStation(object nonDefault)
		{
			this._ = "non-default";
		}

		private readonly string _;
		
		#region Methods/Operators

		protected override INinnelStream CoreProcess(NinnelStationFrame data, INinnelStream target, NinnelMiddlewareDelegate<NinnelStationFrame, INinnelStream> next)
		{
			INinnelStream newNinnelStream;

			if ((object)data.NinnelContext == null)
				throw new NinnelException(nameof(data.NinnelContext));

			if ((object)data.RecordConfiguration == null)
				throw new NinnelException(nameof(data.RecordConfiguration));

			Console.WriteLine(string.Format("DEMO: {0} intermediate station BEFORE", this._));

			if ((object)next != null)
				newNinnelStream = next(data, target);
			else
				newNinnelStream = target;

			Console.WriteLine(string.Format("DEMO: {0} intermediate station AFTER", this._));

			return newNinnelStream;
		}

		#endregion

		protected override IUnknownSolderConfiguration<_DemoSpecification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration)
		{
			return new UnknownNinnelConfiguration<_DemoSpecification>(untypedUnknownSolderConfiguration);
		}
	}
}