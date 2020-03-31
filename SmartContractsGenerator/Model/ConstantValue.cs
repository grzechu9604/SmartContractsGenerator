using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;

namespace SmartContractsGenerator.Model
{
    public class ConstantValue : IAssignable
    {
        public string Value { get; set; }

        public string GenerateCode()
        {
            if (Value == null)
            {
                throw new MissingMandatoryElementException("Value in ConstantValue is required!");
            }

            return Value;
        }
    }
}
