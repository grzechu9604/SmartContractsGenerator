using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;

namespace SmartContractsGenerator.Model
{
    public class Requirement : ICodeGenerator
    {
        public Condition Condition { get; set; }
        public string ErrorMessage { get; set; }

        public virtual string GenerateCode()
        {
            if (Condition == null)
            {
                throw new MissingMandatoryElementException("Condition is mandatory element of requirement");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                return $"require({Condition.GenerateCode()}, {ErrorMessage});\n";
            }
            else
            {
                return $"require({Condition.GenerateCode()});\n";
            }
        }
    }
}
