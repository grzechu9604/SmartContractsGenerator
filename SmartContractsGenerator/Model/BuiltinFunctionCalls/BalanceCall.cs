using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.BuiltinFunctionCalls
{
    public class BalanceCall : IAssignable, ICodeGenerator
    {
        public IAssignable Address { get; set; }

        public string GenerateCode()
        {
            if (Address == null)
            {
                throw new MissingMandatoryElementException("Address is obligatory element of Balance");
            }

            return $"payable({Address.GenerateCode()}).balance";
        }
    }
}
