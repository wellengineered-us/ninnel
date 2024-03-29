/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.Ninnel.Primitives
{
	/// <summary>
	/// The exception thrown when a Ninnel runtime error occurs.
	/// </summary>
	public sealed class NinnelException : Exception
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the NinnelException class.
		/// </summary>
		public NinnelException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the NinnelException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public NinnelException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the NinnelException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public NinnelException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}