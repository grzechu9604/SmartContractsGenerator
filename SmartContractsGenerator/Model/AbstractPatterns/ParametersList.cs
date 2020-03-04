using SmartContractsGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public class ParametersList : ICodeGenerator
    {
        public List<Parameter> Parameters { get; set; }

        public virtual string GenerateCode()
        {
            return Parameters != null ? string.Join(", ", Parameters.Select(p => p.GenerateCode())) : string.Empty;
        }
    }
}
