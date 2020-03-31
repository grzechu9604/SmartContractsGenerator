using SmartContractsGenerator.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public class InstructionsList
    {
        private readonly List<IInstruction> _instructions = new List<IInstruction>();

        public virtual string GenerateCode()
        {
            return string.Join("\n", _instructions.Select(p => p.GenerateCode()));
        }

        public virtual void AppendInstruction(IInstruction instruction)
        {
            _instructions.Add(instruction);
        }

        public virtual bool Any() => _instructions.Any();

        public virtual bool ContainsOnlyIf() => _instructions.Count == 1 && _instructions.First() is IfStatement;
    }
}
