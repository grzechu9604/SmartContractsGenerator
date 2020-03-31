using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using System;

namespace SmartContractsGenerator.Model
{
    public class Condition : ICodeGenerator
    {
        public Operation ConditionOperation { get; set; }

        public virtual string GenerateCode()
        {
            if (ConditionOperation == null)
            {
                throw new MissingMandatoryElementException("Condition operation is mandatory in Condition");
            }

            return ConditionOperation.GenerateCode();
        }
    }
}
