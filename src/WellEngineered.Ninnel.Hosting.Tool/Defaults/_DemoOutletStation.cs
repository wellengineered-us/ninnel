/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Ninnel.Station;
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Hosting.Tool.Defaults
{
	public partial class _DemoOutletStation : NinnelOutletStation<_DemoSpecification>
	{
		public _DemoOutletStation()
		{
		}

		protected override IUnknownSolderConfiguration<_DemoSpecification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration)
		{
			return new UnknownNinnelConfiguration<_DemoSpecification>(untypedUnknownSolderConfiguration);
		}

		protected override void CorePostExecute(NinnelStationFrame ninnelStationFrame)
		{
			Console.WriteLine(string.Format("DEMO: outlet station POST-EXECUTE"));
		}

		protected override void CorePreExecute(NinnelStationFrame ninnelStationFrame)
		{
			Console.WriteLine(string.Format("DEMO: outlet station PRE-EXECUTE"));
		}

		protected override void CoreDeliver(NinnelStationFrame ninnelStationFrame, INinnelStream ninnelStream)
		{
			Console.WriteLine(string.Format("DEMO: outlet station DELIVER"));
		}
	}
}