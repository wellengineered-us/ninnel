/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Threading.Tasks;

using WellEngineered.Ninnel.Primitives.Component;

namespace WellEngineered.Ninnel.Middleware
{
	public delegate ValueTask<TComponent> AsyncNinnelMiddlewareDelegate<in TData, TComponent>(TData data, TComponent target)
		where TComponent : INinnelComponent0;
}