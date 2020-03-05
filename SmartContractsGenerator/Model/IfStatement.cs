using SmartContractsGenerator.Model.AbstractPatterns;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class IfStatement : Instruction
    {
        public InstructionsList TrueInstructions { get; set; }
        public InstructionsList FalseInstructions { get; set; }
        public Condition Condition { get; set; }

        public override string GenerateCode()
        {
            StringBuilder codeBuilder = new StringBuilder();

            codeBuilder.Append($"if ({Condition.GenerateCode()}) {{\n");
            codeBuilder.Append($"{TrueInstructions.GenerateCode()}\n}}");

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
