using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Exceptions
{
    public class MissingMandatoryElementException : Exception
    {
        public MissingMandatoryElementException(string message) : base(message)
        {
        }

        public MissingMandatoryElementException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MissingMandatoryElementException()
        {
        }
    }
}
