/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Middleware
{
	public static partial class NinnelMiddlewareBuilderExtensions
	{
		#region Methods/Operators

		public static INinnelMiddlewareBuilder<TData, TComponent> From<TData, TComponent, TConfiguration>(this INinnelMiddlewareBuilder<TData, TComponent> ninnelMiddlewareBuilder, Type ninnelMiddlewareType, TConfiguration ninnelMiddlewareConfiguration, bool autoWire, string selectorKey = null, bool throwOnError = true)
			where TComponent : ILifecycle
			where TConfiguration : class, INinnelConfiguration
		{
			if (ninnelMiddlewareBuilder == null)
				throw new ArgumentNullException(nameof(ninnelMiddlewareBuilder));

			if (ninnelMiddlewareType == null)
				throw new ArgumentNullException(nameof(ninnelMiddlewareType));

			if (ninnelMiddlewareConfiguration == null)
				throw new ArgumentNullException(nameof(ninnelMiddlewareConfiguration));

			return ninnelMiddlewareBuilder.Use(next =>
												{
													return (data, target) =>
															{
																TComponent newTarget;
																Type _ninnelMiddlewareType = ninnelMiddlewareType; // prevent closure bug
																TConfiguration _ninnelMiddlewareConfiguration = ninnelMiddlewareConfiguration; // prevent closure bug
																bool _autoWire = autoWire; // prevent closure bug
																string _selectorKey = selectorKey; // prevent closure bug
																bool _throwOnError = throwOnError; // prevent closure bug
																INinnelMiddleware<TData, TComponent, TConfiguration> ninnelMiddleware;

																if (data == null)
																	throw new ArgumentNullException(nameof(data));

																//if (target == null)
																//throw new ArgumentNullException(nameof(target));

																if (_ninnelMiddlewareType == null)
																	throw new InvalidOperationException(nameof(_ninnelMiddlewareType));

																if (_ninnelMiddlewareConfiguration == null)
																	throw new InvalidOperationException(nameof(_ninnelMiddlewareConfiguration));

																// TODO - Fix this
																ninnelMiddleware = new DefaultComponentFactory()
																	.CreateNinnelComponent<INinnelMiddleware<TData, TComponent, TConfiguration>>(AssemblyDomain.Default.DependencyManager, _ninnelMiddlewareType, _autoWire, _selectorKey, _throwOnError);

																if ((object)ninnelMiddleware == null)
																	throw new InvalidOperationException(nameof(ninnelMiddleware));

																using (ninnelMiddleware)
																{
																	ninnelMiddleware.Configuration = _ninnelMiddlewareConfiguration;
																	ninnelMiddleware.Configuration.ValidateFail("Middleware");
																	ninnelMiddleware.Create();

																	//ninnelMiddleware.PreEx()
																	newTarget = ninnelMiddleware.Process(data, target, next);
																	//ninnelMiddleware.PreEx()
																		
																	return newTarget;
																}
															};
												});
		}

		public static INinnelMiddlewareBuilder<TData, TComponent> With<TData, TComponent, TConfiguration>(this INinnelMiddlewareBuilder<TData, TComponent> ninnelMiddlewareBuilder, INinnelMiddleware<TData, TComponent, TConfiguration> ninnelMiddleware)
			where TComponent : ILifecycle
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

																//if (target == null)
																//throw new ArgumentNullException(nameof(target));

																if (_ninnelMiddleware == null)
																	throw new InvalidOperationException(nameof(_ninnelMiddleware));
																
																using (_ninnelMiddleware)
																{
																	if (!_ninnelMiddleware.IsCreated)
																	{
																		_ninnelMiddleware.Configuration.ValidateFail("Middleware");
																		_ninnelMiddleware.Create();
																	}

																	if (_ninnelMiddleware.IsDisposed)
																		newTarget = default;
																	else
																		newTarget = _ninnelMiddleware.Process(data, target, next);

																	return newTarget;
																}
															};
												});
		}

		#endregion

		/*public static INinnelMiddlewareBuilder<object, ILifecycle> With(this INinnelMiddlewareBuilder<object, ILifecycle> ninnelMiddlewareBuilder, INinnelMiddleware ninnelMiddleware)
		{
			if (ninnelMiddlewareBuilder == null)
				throw new ArgumentNullException(nameof(ninnelMiddlewareBuilder));

			if (ninnelMiddleware == null)
				throw new ArgumentNullException(nameof(ninnelMiddleware));

			return ninnelMiddlewareBuilder.Use(next =>
												{
													return (data, target) =>
															{
																ILifecycle newTarget;
																INinnelMiddleware _ninnelMiddleware = ninnelMiddleware; // prevent closure bug

																if (data == null)
																	throw new ArgumentNullException(nameof(data));

																//if (target == null)
																//throw new ArgumentNullException(nameof(target));

																if (_ninnelMiddleware == null)
																	throw new InvalidOperationException(nameof(_ninnelMiddleware));

																using (_ninnelMiddleware)
																{
																	if (!_ninnelMiddleware.IsCreated || _ninnelMiddleware.IsDisposed)
																		newTarget = default;
																	else
																		newTarget = _ninnelMiddleware.Process(data, target, (d, c) => next(d, c));

																	return newTarget;
																}
															};
												});
		}*/
	}
}