using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class Modifier : AbstractContainer
    {
        public ParametersList Parameters { get; set; }
        public string Name
        {
            get => name;
            set
            {
                if (NameValidator.IsValidName(value))
                {
                    name = value;
                }
                else
                {
                    throw new InvalidOperationException("Defined contract has invalid name");
                }
            }
        }
        private string name;

        protected override string GetContent()
        {
            return string.Empty;
        }

        protected override string GetFooter() => "_;\n}";

        protected override string GetHeader()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingMandatoryElementException("Name is mandatory element of modifier");
            }

            return $"modifier {Name}({Parameters?.GenerateCode()}) {{\n";
        }
    }
}
