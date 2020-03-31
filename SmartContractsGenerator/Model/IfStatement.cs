using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class IfStatement : IInstruction
    {
        public InstructionsList TrueInstructions { get; set; }
        public InstructionsList FalseInstructions { get; set; }
        public Condition Condition { get; set; }

        public virtual string GenerateCode()
        {
            if (Condition == null)
            {
                throw new MissingMandatoryElementException("Condition is required in if statement!");
            }

            StringBuilder codeBuilder = new StringBuilder();

            codeBuilder.Append($"if ({Condition.GenerateCode()}) {{");

            if (TrueInstructions != null)
            {
                codeBuilder.Append($"\n{TrueInstructions.GenerateCode()}");
            }

            codeBuilder.Append("\n}");


            if (FalseInstructions != null && FalseInstructions.Any())
            {
                if (FalseInstructions.ContainsOnlyIf())
                {
                    codeBuilder.Append($" else {FalseInstructions.GenerateCode()}");
                }
                else
                {
                    codeBuilder.Append($" else {{\n");
                    codeBuilder.Append($"{FalseInstructions.GenerateCode()}\n}}");
                }
            }

            return codeBuilder.ToString();
        }
    }
}
