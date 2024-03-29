/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Hosting
{
	public abstract partial class NinnelHostFactory
		: NinnelComponent0,
			INinnelHostFactory
	{
		#region Constructors/Destructors

		protected NinnelHostFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract INinnelHost CoreCreateHost();

		public INinnelHost CreateHost()
		{
			try
			{
				return this.CoreCreateHost();
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The host factory failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}