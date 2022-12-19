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
	public partial class _DemoInletStation : NinnelInletStation<_DemoSpecification>
	{
		public _DemoInletStation()
		{
		}

		protected override IUnknownSolderConfiguration<_DemoSpecification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration)
		{
			return new UnknownNinnelConfiguration<_DemoSpecification>(untypedUnknownSolderConfiguration);
		}

		protected override INinnelStream CoreInject(NinnelStationFrame ninnelStationFrame)
		{
			Console.WriteLine(string.Format("DEMO: inlet station INJECT"));
			return null;
		}

		protected override void CorePostExecute(NinnelStationFrame ninnelStationFrame)
		{
			Console.WriteLine(string.Format("DEMO: inlet station POST-EXECUTE"));
		}

		protected override void CorePreExecute(NinnelStationFrame ninnelStationFrame)
		{
			Console.WriteLine(string.Format("DEMO: inlet station PRE-EXECUTE"));
		}
	}
}