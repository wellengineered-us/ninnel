/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Material;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Injection;

namespace WellEngineered.Ninnel.Station.Minimal
{
	public partial class NullInletStation
		: NinnelInletStation<EmptySpecification>
	{
		#region Constructors/Destructors

		public NullInletStation()
		{
		}

		#endregion

		#region Methods/Operators

		protected override IUnknownSolderConfiguration<EmptySpecification> CoreCreateGenericTypedUnknownConfiguration(IUnknownSolderConfiguration untypedUnknownSolderConfiguration)
		{
			return new UnknownNinnelConfiguration<EmptySpecification>(untypedUnknownSolderConfiguration);
		}

		protected override INinnelStream CoreInject(NinnelStationFrame ninnelStationFrame)
		{
			AssemblyDomain.Default.ResourceManager.Print(Guid.Empty, string.Format("{0}", nameof(this.CoreInject)));
			return null;
		}

		protected override void CorePostExecute(NinnelStationFrame ninnelStationFrame)
		{
			AssemblyDomain.Default.ResourceManager.Print(Guid.Empty, string.Format("{0}", nameof(this.CorePostExecute)));
		}

		protected override void CorePreExecute(NinnelStationFrame ninnelStationFrame)
		{
			AssemblyDomain.Default.ResourceManager.Print(Guid.Empty, string.Format("{0}", nameof(this.CorePreExecute)));
		}

		#endregion
	}
}