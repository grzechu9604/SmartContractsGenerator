using SmartContractsGenerator.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public class CallingParametersList : ICodeGenerator
    {
        public List<IAssignable> Parameters { get; set; }

        public virtual string GenerateCode()
        {
            return Parameters != null ? string.Join(", ", Parameters.Select(p => p.GenerateCode())) : string.Empty;
        }

        public virtual bool AnyParameter() => Parameters != null && Parameters.Any();
    }
}
