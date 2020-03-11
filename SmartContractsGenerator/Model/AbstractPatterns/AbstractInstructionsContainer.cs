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
            return Instructions?.GenerateCode();
        }
    }
}
