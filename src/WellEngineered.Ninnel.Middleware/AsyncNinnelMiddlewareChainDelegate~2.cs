/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;

namespace WellEngineered.Ninnel.Middleware
{
	// should NOT return ValueTask<TOutput>
	public delegate TOutput AsyncNinnelMiddlewareChainDelegate<in TInput, out TOutput>(TInput input);
}
#endif