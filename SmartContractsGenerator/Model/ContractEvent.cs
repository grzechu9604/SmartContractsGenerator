using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Validators;
using System;

namespace SmartContractsGenerator.Model
{
    public class ContractEvent : ICallable, ICodeGenerator
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
                    throw new InvalidOperationException("Defined event has invalid name");
                }
            }
        }
        private string name;
        public ParametersList Parameters { get; set; }

        public virtual string GenerateCode()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingMandatoryElementException("Name is mandatory element of event");
            }

            return $"event {Name}({Parameters?.GenerateCode()})";
        }

        public virtual string GenerateCallCode()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingMandatoryElementException("Name is mandatory element of event");
            }

            return Name;
        }
    }
}
