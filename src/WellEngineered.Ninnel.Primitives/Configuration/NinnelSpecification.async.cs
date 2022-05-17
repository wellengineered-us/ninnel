/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using WellEngineered.Solder.Configuration;

namespace WellEngineered.Ninnel.Primitives.Configuration
{
	public abstract partial class NinnelSpecification
		: SolderSpecification,
			INinnelSpecification
	{
	}
}
#endif