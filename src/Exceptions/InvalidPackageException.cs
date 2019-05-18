using System;

namespace PackageStore.Exceptions
{
  public class InvalidPackageException : Exception
  {
    public InvalidPackageException(string message) :
      base(message)
    {
    }

    public InvalidPackageException(string message, Exception innerException) :
      base(message, innerException)
    {
    }
  }
}
