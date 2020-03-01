using SmartContractsGenerator.Model.AbstractPatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class Modifier : AbstractContainer
    {
        public ParametersList Parameters { get; set; }
        public string Name { get; set; }

        protected override string GetContent()
        {
            return string.Empty;
        }

        protected override string GetFooter() => "_;\n}";

        protected override string GetHeader() =>  $"modifier {Name}({Parameters?.GenerateCode()}) {{\n";
    }
}
