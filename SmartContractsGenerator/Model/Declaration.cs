using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;

namespace SmartContractsGenerator.Model
{
    public class Declaration : IValueContainer, IOneLineInstruction
    {
        public Variable Variable { get; set; }
        public virtual string GenerateCode()
        {
            if (Variable == null)
            {
                throw new MissingMandatoryElementException("Variable is required for Declaration");
            }

            return Variable.GenerateDeclarationCode();
        }
    }
}
