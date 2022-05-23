/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Ninnel.Primitives;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Configuration
{
	public static partial class ConfigurationExtensions
	{
		public static void ValidateFail<TNinnelConfiguration>(this TNinnelConfiguration ninnelConfiguration, object context)
			where TNinnelConfiguration : INinnelConfiguration
		{
			IEnumerable<IMessage> messages;

			if ((object)ninnelConfiguration == null)
				throw new ArgumentNullException(nameof(ninnelConfiguration));

			messages = ninnelConfiguration.Validate(context);

			if ((object)messages != null)
			{
				int count = 0;
				foreach (IMessage message in messages)
				{
					if (message == null)
						continue;

					Console.Out.WriteLine(string.Format("{0}[{1}] => {2}", message.Severity, (count + 1), message.Description));

					count++;
				}

				if (count > 0)
					throw new NinnelException(string.Format("{0} configuration validation failed with error count: {1}", context, count));
			}
		}
	}
}