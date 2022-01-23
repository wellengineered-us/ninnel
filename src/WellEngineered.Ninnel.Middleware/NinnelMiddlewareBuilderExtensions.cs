/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Primitives.Configuration;

namespace WellEngineered.Ninnel.Middleware
{
	public static partial class NinnelMiddlewareBuilderExtensions
	{
		#region Methods/Operators

		public static INinnelMiddlewareBuilder<TData, TComponent> From<TData, TComponent, TConfiguration>(this INinnelMiddlewareBuilder<TData, TComponent> ninnelMiddlewareBuilder, Type ninnelMiddlewareClass, TConfiguration ninnelMiddlewareConfiguration)
			where TComponent : INinnelComponent0
			where TConfiguration : class, INinnelConfiguration
		{
			if (ninnelMiddlewareBuilder == null)
				throw new ArgumentNullException(nameof(ninnelMiddlewareBuilder));

			if (ninnelMiddlewareClass == null)
				throw new ArgumentNullException(nameof(ninnelMiddlewareClass));

			if (ninnelMiddlewareConfiguration == null)
				throw new ArgumentNullException(nameof(ninnelMiddlewareConfiguration));

			return ninnelMiddlewareBuilder.Use(next =>
												{
													return (data, target) =>
															{
																TComponent newTarget;
																Type _ninnelMiddlewareClass = ninnelMiddlewareClass; // prevent closure bug
																TConfiguration _ninnelMiddlewareConfiguration = ninnelMiddlewareConfiguration; // prevent closure bug
																INinnelMiddleware<TData, TComponent, TConfiguration> ninnelMiddleware;

																if (data == null)
																	throw new ArgumentNullException(nameof(data));

																if (target == null)
																	throw new ArgumentNullException(nameof(target));

																if (_ninnelMiddlewareClass == null)
																	throw new InvalidOperationException(nameof(_ninnelMiddlewareClass));

																if (_ninnelMiddlewareConfiguration == null)
																	throw new InvalidOperationException(nameof(_ninnelMiddlewareConfiguration));

																ninnelMiddleware = (INinnelMiddleware<TData, TComponent, TConfiguration>)Activator.CreateInstance(_ninnelMiddlewareClass);

																if ((object)ninnelMiddleware == null)
																	throw new InvalidOperationException(nameof(ninnelMiddleware));

																using (ninnelMiddleware)
																{
																	ninnelMiddleware.Configuration = _ninnelMiddlewareConfiguration;
																	//ninnelMiddleware.Configuration.Validate();
																	ninnelMiddleware.Create();

																	newTarget = ninnelMiddleware.Process(data, target, next);

																	return newTarget;
																}
															};
												});
		}

		public static INinnelMiddlewareBuilder<TData, TComponent> With<TData, TComponent, TConfiguration>(this INinnelMiddlewareBuilder<TData, TComponent> ninnelMiddlewareBuilder, INinnelMiddleware<TData, TComponent, TConfiguration> ninnelMiddleware)
			where TComponent : INinnelComponent0
			where TConfiguration : class, INinnelConfiguration
		{
			if (ninnelMiddlewareBuilder == null)
				throw new ArgumentNullException(nameof(ninnelMiddlewareBuilder));

			if (ninnelMiddleware == null)
				throw new ArgumentNullException(nameof(ninnelMiddleware));

			return ninnelMiddlewareBuilder.Use(next =>
												{
													return (data, target) =>
															{
																TComponent newTarget;
																INinnelMiddleware<TData, TComponent, TConfiguration> _ninnelMiddleware = ninnelMiddleware; // prevent closure bug

																if (data == null)
																	throw new ArgumentNullException(nameof(data));

																if (target == null)
																	throw new ArgumentNullException(nameof(target));

																if (_ninnelMiddleware == null)
																	throw new InvalidOperationException(nameof(_ninnelMiddleware));

																if (!_ninnelMiddleware.IsCreated || _ninnelMiddleware.IsDisposed)
																	newTarget = default;
																else
																	newTarget = _ninnelMiddleware.Process(data, target, next);

																return newTarget;
															};
												});
		}

		#endregion
	}
}