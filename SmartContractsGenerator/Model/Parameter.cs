using SmartContractsGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class Parameter : ICodeGenerator
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public virtual string GenerateCode()
        {
            return $"{Type} {Name}";
        }
    }
}
