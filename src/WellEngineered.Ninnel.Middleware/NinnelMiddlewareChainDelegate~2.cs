/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Ninnel.Middleware
{
	public delegate TOutput NinnelMiddlewareChainDelegate<in TInput, out TOutput>(TInput input);
}