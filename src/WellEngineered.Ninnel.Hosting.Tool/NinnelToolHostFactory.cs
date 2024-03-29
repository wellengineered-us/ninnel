/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public abstract partial class NinnelToolHostFactory
		: NinnelHostFactory,
			INinnelToolHostFactory
	{
		#region Constructors/Destructors

		public NinnelToolHostFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract INinnelToolHost CoreCreateHost(ToolHostConfiguration toolHostConfiguration);

		protected abstract INinnelToolHost CoreCreateHost(Uri toolHostConfigUri);

		public INinnelToolHost CreateToolHost(ToolHostConfiguration toolHostConfiguration)
		{
			if ((object)toolHostConfiguration == null)
				throw new ArgumentNullException(nameof(toolHostConfiguration));

			try
			{
				return this.CoreCreateHost(toolHostConfiguration);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The tool host factory failed (see inner exception)."), ex);
			}
		}

		public INinnelToolHost CreateToolHost(Uri toolHostConfigUri)
		{
			if ((object)toolHostConfigUri == null)
				throw new ArgumentNullException(nameof(toolHostConfigUri));

			try
			{
				return this.CoreCreateHost(toolHostConfigUri);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The tool host factory failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}