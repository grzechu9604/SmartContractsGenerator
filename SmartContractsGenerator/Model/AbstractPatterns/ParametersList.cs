using SmartContractsGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public class ParametersList : ICodeGenerable
    {
        public List<Parameter> Parameters { get; set; }

        public string GenerateCode()
        {
            return Parameters != null ? string.Join(", ", Parameters.Select(p => p.GenerateCode())) : string.Empty;
        }
    }
}
