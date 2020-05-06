using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;

namespace SmartContractsGenerator.Model
{
    public class ReturnStatement : IInstruction
    {
        public IValueContainer ToReturn { get; set; }

        public string GenerateCode()
        {
            if (ToReturn == null)
            {
                throw new MissingMandatoryElementException("Object to return is mandatory element of return statement");
            }

            return $"return {ToReturn.GenerateCode()}";
        }
    }
}
