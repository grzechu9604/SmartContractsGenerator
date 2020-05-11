using SmartContractsGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public class InstructionsList : ICodeGeneratorWithIndentation
    {
        private readonly List<IInstruction> _instructions = new List<IInstruction>();

        public virtual string GenerateCode(Indentation indentation)
        {
            return string.Join("\n", _instructions.Select(i => $"{indentation?.GenerateCode()}{GenerateChildCode(i, indentation)}{GetInstructionEnding(i)}"));
        }

        public virtual void AppendInstruction(IInstruction instruction)
        {
            _instructions.Add(instruction);
        }

        public static string GetInstructionEnding(IInstruction i) => i is IOneLineInstruction ? ";" : string.Empty;

        public virtual bool Any() => _instructions.Any();

        public virtual bool ContainsOnlyIf() => _instructions.Count == 1 && _instructions.First() is IfStatement;

        public virtual string GenerateChildCode(IInstruction instruction, Indentation indentation)
        {
            if (instruction is ICodeGenerator codeGenerator)
            {
                return codeGenerator.GenerateCode();
            }

            if (instruction is ICodeGeneratorWithIndentation codeGeneratorWithIndentation)
            {
                return codeGeneratorWithIndentation.GenerateCode(indentation);
            }

            throw new InvalidOperationException("Element of Instruction list must be ICodeGenerator or ICodeGeneratorWithIndentation");
        }

        public IInstruction GetFirstInstruction() => _instructions.First();
    }
}
