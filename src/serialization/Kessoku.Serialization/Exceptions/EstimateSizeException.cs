using System;
using System.Collections.Generic;
using System.Text;

namespace Kessoku.Serialization.Exceptions
{
	/// <summary>
	/// Exception, throwed when we fall to get object size
	/// </summary>
	[Serializable]
	public class EstimateSizeException : Exception
	{
		public EstimateSizeException() { }
		public EstimateSizeException(string message) : base(message) { }
		public EstimateSizeException(string message, Exception inner) : base(message, inner) { }
		protected EstimateSizeException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
