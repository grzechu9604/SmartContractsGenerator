using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Model.Enums;
using SmartContractsGenerator.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class ContractFunction : AbstractInstructionsContainer
    {
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

        public Visibility? Visibility { get; set; }

        public ParametersList Parameters { get; set; }

        protected override string GetHeader()
        {
            if (!Visibility.HasValue)
            {
                throw new MissingMandatoryElementException("Visibility specifier is required for function");
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingMandatoryElementException("Name is required for function");
            }

            return $"function {Name}({Parameters?.GenerateCode()}) {Visibility.Value.GenerateCode()} {{\n";
        }
    }
}
