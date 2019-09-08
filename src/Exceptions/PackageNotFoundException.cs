using System;

namespace PackageStore.Exceptions
{
	public class PackageNotFoundException : Exception
	{
		public PackageNotFoundException(string message) : base(message)
		{
		}

		public PackageNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
