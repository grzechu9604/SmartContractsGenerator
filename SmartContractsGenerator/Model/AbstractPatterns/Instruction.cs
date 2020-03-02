using SmartContractsGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public abstract class Instruction : ICodeGenerable
    {
        public abstract string GenerateCode();
    }
}
