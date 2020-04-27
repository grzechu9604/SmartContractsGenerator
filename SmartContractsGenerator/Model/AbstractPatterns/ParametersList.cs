using SmartContractsGenerator.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public class ParametersList : ICodeGenerator
    {
        public List<Variable> Parameters { get; set; }

        public virtual string GenerateCode()
        {
            return Parameters != null ? string.Join(", ", Parameters.Select(p => p.GenerateDeclarationCode())) : string.Empty;
        }

        public virtual string GenerateCallCode()
        {
            return Parameters != null ? string.Join(", ", Parameters.Select(p => p.GenerateCode())) : string.Empty;
        }

        public virtual bool AnyParameter() => Parameters != null && Parameters.Any();
    }
}
