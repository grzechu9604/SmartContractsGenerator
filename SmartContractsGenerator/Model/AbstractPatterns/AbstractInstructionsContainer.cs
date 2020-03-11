using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public abstract class AbstractInstructionsContainer : AbstractContainer
    {
        public InstructionsList Instructions { get; set; }

        protected override string GetContent()
        {
            if (Instructions != null && Instructions.Any())
            {
                return Instructions.GenerateCode() + '\n';
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
