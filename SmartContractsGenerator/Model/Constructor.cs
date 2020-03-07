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
        public string Visibility { get; set; }
        public ParametersList Parameters { get; set; }

        public InstructionsList Instructions { get; set; }

        protected override string GetContent()
        {
            return Instructions?.GenerateCode();
        }

        protected override string GetHeader() => $"constructor({Parameters?.GenerateCode()}) {Visibility} {{\n";
    }
}
