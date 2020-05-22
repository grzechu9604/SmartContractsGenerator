using SmartContractsGenerator.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public class ParametersList
    {
        public List<Variable> Parameters { get; set; }

        public virtual string GenerateCode(bool pointStorageType)
        {
            return Parameters != null ? string.Join(", ", Parameters.Select(p => p.GenerateDeclarationCode(pointStorageType))) : string.Empty;
        }

        public virtual bool AnyParameter() => Parameters != null && Parameters.Any();
    }
}
