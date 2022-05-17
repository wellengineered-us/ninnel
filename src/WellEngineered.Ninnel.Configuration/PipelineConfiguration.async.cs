/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Configuration
{
	public partial class PipelineConfiguration
		: NinnelConfiguration
	{
		#region Methods/Operators

		protected override async IAsyncEnumerable<IMessage> CoreValidateAsync(object context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			Type ninnelPipelineType;

			if (string.IsNullOrWhiteSpace(this.PipelineAssemblyQualifiedTypeName))
				yield return new Message(string.Empty, string.Format("{0} assembly qualified type name is required.", context), Severity.Error);
			else
			{
				ninnelPipelineType = this.GetPipelineType();

				if ((object)ninnelPipelineType == null)
					yield return new Message(string.Empty, string.Format("{0} assembly qualified type name failed to load.", context), Severity.Error);
				/*else if (!typeof(INinnelHost).IsAssignableFrom(ninnelHostType))
					yield return new Message(string.Empty, string.Format("{0} assembly qualified type name loaded an unrecognized type.", context), Severity.Error);
				else
				{
					ninnelHost = ReflectionExtensions.CreateInstanceAssignableToTargetType<INinnelHost>(ninnelHostType);

					if ((object)ninnelHost == null)
						yield return new Message(string.Empty, string.Format("{0} assembly qualified type name failed to instantiate type.", context), Severity.Error);
				}*/
			}

			if ((object)this.InletStationConfiguration == null)
				yield return new Message(string.Empty, string.Format("Inlet station configuration is required."), Severity.Error);
			else
			{
				var childMessages = this.InletStationConfiguration.ValidateAsync(string.Format("{0}/{1}", context, "Inlet station"), cancellationToken);

				await foreach (var childMessage in childMessages.WithCancellation(cancellationToken))
				{
					yield return childMessage;
				}
			}

			if ((object)this.IntermediateStationConfigurations == null)
				yield return new Message(string.Empty, string.Format("Intermediate station configurations are required."), Severity.Error);
			else
			{
				var childMessages = this.IntermediateStationConfigurations.ValidateAsync(string.Format("{0}/{1}", context, "Intermediate station"), cancellationToken);

				await foreach (var childMessage in childMessages.WithCancellation(cancellationToken))
				{
					yield return childMessage;
				}
			}

			if ((object)this.OutletStationConfiguration == null)
				yield return new Message(string.Empty, string.Format("Outlet station configuration is required."), Severity.Error);
			else
			{
				var childMessages = this.OutletStationConfiguration.ValidateAsync(string.Format("{0}/{1}", context, "Outlet station"), cancellationToken);

				await foreach (var childMessage in childMessages.WithCancellation(cancellationToken))
				{
					yield return childMessage;
				}
			}
		}

		#endregion
	}
}
#endif