using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Validators;
using System;

namespace SmartContractsGenerator.Model
{
    public class Modifier : AbstractInstructionsContainer, ICallable
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
                    throw new InvalidOperationException($"Defined modifier has invalid name - {value}");
                }
            }
        }
        private string name;

        protected override string GetFooterPrefix() => "_;";

        protected override string GetHeader()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingMandatoryElementException("Name is mandatory element of modifier");
            }

            return $"modifier {Name}({Parameters?.GenerateCode(true)}) {{\n";
        }

        public string GenerateCallCode()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingMandatoryElementException("Name is mandatory element of modifier");
            }

            return Name;
        }
    }
}
