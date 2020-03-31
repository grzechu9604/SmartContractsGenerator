using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;

namespace SmartContractsGenerator.Model
{
    public class ContractLoop : AbstractInstructionsContainer, IInstruction
    {
        public Declaration InitialDeclaration { get; set; }
        public Condition BreakCondition { get; set; }
        public IOneLineInstruction StepInstruction { get; set; }
        protected override string GetHeader()
        {
            if (InitialDeclaration == null)
            {
                throw new MissingMandatoryElementException("Initial declaration is required in loop statement");
            }

            if (BreakCondition == null)
            {
                throw new MissingMandatoryElementException("Break condition is required in loop statement");
            }

            if (StepInstruction == null)
            {
                throw new MissingMandatoryElementException("Step instruction is required in loop statement");
            }

            return $"for ({InitialDeclaration.GenerateCode()}; {BreakCondition.GenerateCode()}; {StepInstruction.GenerateCode()}) {{\n";
        }
    }
}
