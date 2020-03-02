using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public class InstructionsList
    {
        private readonly List<Instruction> _instructions = new List<Instruction>();

        public string GenerateCode()
        {
            return string.Join("\n", _instructions.Select(p => p.GenerateCode()));
        }

        public void AppendInstruction(Instruction instruction)
        {
            _instructions.Add(instruction);
        }
    }
}
