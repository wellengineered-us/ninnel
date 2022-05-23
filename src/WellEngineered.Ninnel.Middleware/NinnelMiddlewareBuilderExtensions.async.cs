/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Threading;

using WellEngineered.Ninnel.Configuration;
using WellEngineered.Ninnel.Primitives.Component;
using WellEngineered.Ninnel.Primitives.Configuration;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Primitives;

namespace WellEngineered.Ninnel.Middleware
{
	public static partial class AsyncNinnelMiddlewareBuilderExtensions
	{
		#region Methods/Operators

		public static IAsyncNinnelMiddlewareBuilder<TData, TComponent> FromAsync<TData, TComponent, TConfiguration>(this IAsyncNinnelMiddlewareBuilder<TData, TComponent> ninnelMiddlewareBuilder, Type ninnelMiddlewareType, TConfiguration ninnelMiddlewareConfiguration, bool autoWire)
			where TComponent : IAsyncLifecycle
			where TConfiguration : class, INinnelConfiguration
		{
			if (ninnelMiddlewareBuilder == null)
				throw new ArgumentNullException(nameof(ninnelMiddlewareBuilder));

			if (ninnelMiddlewareType == null)
				throw new ArgumentNullException(nameof(ninnelMiddlewareType));

			if (ninnelMiddlewareConfiguration == null)
				throw new ArgumentNullException(nameof(ninnelMiddlewareConfiguration));

			return ninnelMiddlewareBuilder.UseAsync(next =>
													{
														return async (data, target) =>
																{
																	TComponent newTarget;
																	Type _ninnelMiddlewareType = ninnelMiddlewareType; // prevent closure bug
																	TConfiguration _ninnelMiddlewareConfiguration = ninnelMiddlewareConfiguration; // prevent closure bug
																	bool _autoWire = autoWire; // prevent closure bug
																	IAsyncNinnelMiddleware<TData, TComponent, TConfiguration> ninnelMiddleware;

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
																		.CreateNinnelComponent<IAsyncNinnelMiddleware<TData, TComponent, TConfiguration>>(AssemblyDomain.Default.DependencyManager, _ninnelMiddlewareType, autoWire);

																	if ((object)ninnelMiddleware == null)
																		throw new InvalidOperationException(nameof(ninnelMiddleware));

																	await using (ninnelMiddleware)
																	{
																		ninnelMiddleware.Configuration = _ninnelMiddlewareConfiguration;
																		await ninnelMiddleware.Configuration.ValidateFailAsync("Middleware");
																		await ninnelMiddleware.CreateAsync();

																		newTarget = await ninnelMiddleware.ProcessAsync(data, target, next);

																		return newTarget;
																	}
																};
													});
		}

		public static IAsyncNinnelMiddlewareBuilder<TData, TComponent> WithAsync<TData, TComponent, TConfiguration>(this IAsyncNinnelMiddlewareBuilder<TData, TComponent> ninnelMiddlewareBuilder, IAsyncNinnelMiddleware<TData, TComponent, TConfiguration> ninnelMiddleware, CancellationToken cancellationToken = default)
			where TComponent : IAsyncLifecycle
			where TConfiguration : class, INinnelConfiguration
		{
			if (ninnelMiddlewareBuilder == null)
				throw new ArgumentNullException(nameof(ninnelMiddlewareBuilder));

			if (ninnelMiddleware == null)
				throw new ArgumentNullException(nameof(ninnelMiddleware));

			return ninnelMiddlewareBuilder.UseAsync(next =>
													{
														return async (data, target) =>
																{
																	TComponent newTarget;
																	IAsyncNinnelMiddleware<TData, TComponent, TConfiguration> _ninnelMiddleware = ninnelMiddleware; // prevent closure bug
																	CancellationToken _cancellationToken = cancellationToken; // prevent closure bug

																	if (data == null)
																		throw new ArgumentNullException(nameof(data));

																	//if (target == null)
																	//throw new ArgumentNullException(nameof(target));

																	if (_ninnelMiddleware == null)
																		throw new InvalidOperationException(nameof(_ninnelMiddleware));

																	await using (ninnelMiddleware)
																	{
																		if (!_ninnelMiddleware.IsAsyncCreated)
																		{
																			await _ninnelMiddleware.Configuration.ValidateFailAsync("Middleware", _cancellationToken);
																			await _ninnelMiddleware.CreateAsync(_cancellationToken);
																		}
																		
																		if (_ninnelMiddleware.IsAsyncDisposed)
																			newTarget = default;
																		else
																			newTarget = await _ninnelMiddleware.ProcessAsync(data, target, next, _cancellationToken);

																		return newTarget;
																	}
																};
													});
		}

		#endregion
	}
}
#endif