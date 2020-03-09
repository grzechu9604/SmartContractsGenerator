using System;
using System.Collections.Generic;
using System.Text;

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
