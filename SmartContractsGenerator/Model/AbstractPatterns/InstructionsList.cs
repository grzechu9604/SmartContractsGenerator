using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public class InstructionsList
    {
        private readonly List<Instruction> _instructions = new List<Instruction>();

        public virtual string GenerateCode()
        {
            return string.Join("\n", _instructions.Select(p => p.GenerateCode()));
        }

        public virtual void AppendInstruction(Instruction instruction)
        {
            _instructions.Add(instruction);
        }

        public virtual bool Any() => _instructions.Any();

        public virtual bool ContainsOnlyIf() => _instructions.Count == 1 && _instructions.First() is IfStatement;
    }
}
