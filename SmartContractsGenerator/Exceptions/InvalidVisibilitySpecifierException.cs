using System;

namespace SmartContractsGenerator.Exceptions
{
    public class InvalidVisibilitySpecifierException : Exception
    {
        public InvalidVisibilitySpecifierException(string message) : base(message)
        {
        }

        public InvalidVisibilitySpecifierException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidVisibilitySpecifierException()
        {
        }
    }
}
