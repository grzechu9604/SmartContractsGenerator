using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class Constructor : AbstractContainer
    {
        public string Modifier { get; set; }
        public List<Parameter> Parameters { get; set; }

        protected override string GetContent()
        {
            return string.Empty;
        }

        protected override string GetFooter()
        {
            return "}";
        }

        protected override string GetHeader()
        {
            string parametersCode = Parameters != null ? string.Join(", ", Parameters.Select(p => p.GenerateCode())) : string.Empty;
            return $"constructor({parametersCode}) {Modifier} {{\n";
        }
    }
}
