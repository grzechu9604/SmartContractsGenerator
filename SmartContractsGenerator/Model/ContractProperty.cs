using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.Enums;

namespace SmartContractsGenerator.Model
{
    public class ContractProperty : IValueContainer
    {
        public Visibility? Visibility { get; set; }
        public Variable Variable { get; set; }


        public virtual string GenerateCode()
        {
            if (Variable == null)
            {
                throw new MissingMandatoryElementException("Variable is not configured properly");
            }

            return Variable.GenerateCode();
        }

        public virtual string GenerateDeclarationCode()
        {
            if (!Visibility.HasValue)
            {
                throw new MissingMandatoryElementException("Visibility specifier is required for property");
            }

            if (Variable == null || !Variable.Type.HasValue || string.IsNullOrWhiteSpace(Variable.Name))
            {
                throw new MissingMandatoryElementException("Variable in property is not configured properly");
            }

            return $"{Variable.Type.Value.GenerateCode(false)} {Visibility.Value.GenerateCode()} {Variable.Name}";
        }
    }
}
