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
        public ParametersList Parameters { get; set; }

        protected override string GetContent()
        {
            return string.Empty;
        }

        protected override string GetHeader() => $"constructor({Parameters?.GenerateCode()}) {Modifier} {{\n";
    }
}
