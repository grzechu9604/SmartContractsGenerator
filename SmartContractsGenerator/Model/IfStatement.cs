using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class IfStatement : IInstruction, ICodeGeneratorWithIndentation
    {
        public InstructionsList TrueInstructions { get; set; }
        public InstructionsList FalseInstructions { get; set; }
        public Condition Condition { get; set; }

        public virtual string GenerateCode(Indentation indentation)
        {
            if (Condition == null)
            {
                throw new MissingMandatoryElementException("Condition is required in if statement!");
            }

            StringBuilder codeBuilder = new StringBuilder();

            codeBuilder.Append($"if ({Condition.GenerateCode()}) {{");

            if (TrueInstructions != null)
            {
                codeBuilder.Append($"\n{TrueInstructions.GenerateCode(indentation?.GetIndentationWithIncrementedLevel())}");
            }

            codeBuilder.Append($"\n{indentation?.GenerateCode()}}}");


            if (FalseInstructions != null && FalseInstructions.Any())
            {
                if (FalseInstructions.ContainsOnlyIf())
                {
                    var elseIf = FalseInstructions.GetFirstInstruction() as IfStatement;
                     codeBuilder.Append($" else {elseIf.GenerateCode(indentation)}");
                }
                else
                {
                    codeBuilder.Append($" else {{\n");
                    codeBuilder.Append($"{FalseInstructions.GenerateCode(indentation?.GetIndentationWithIncrementedLevel())}\n{indentation?.GenerateCode()}}}");
                }
            }

            return codeBuilder.ToString();
        }
    }
}
