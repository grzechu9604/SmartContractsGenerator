using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Model.Enums;
using SmartContractsGenerator.Validators;
using System;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class ContractFunction : AbstractInstructionsContainer, ICallable
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
        public ParametersList ModifierParameters { get; set; }
        public ModifierAppliance Modifier { get; set; }

        public string ReturningType { get; set; }

        public ModificationType ModificationType { get; set; }

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

            return $"function {Name}({Parameters?.GenerateCode()}) {Visibility.Value.GenerateCode()}{GetModifierHeaderPart()}{GetModificationTypeHeaderPart()}{GetReturnsHeaderPart()} {{\n";
        }

        public virtual string GenerateCallCode()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingMandatoryElementException("Name is mandatory element of contract function");
            }

            return Name;
        }

        public string GetModifierHeaderPart()
        {
            if (Modifier != null)
            {
                return $" {Modifier.GenerateCode()}";
            }
            else
            {
                return null;
            }
        }

        public string GetReturnsHeaderPart() => !string.IsNullOrWhiteSpace(ReturningType) ? $" returns ({ReturningType})" : null;

        public string GetModificationTypeHeaderPart()
        {
            var code = ModificationType.GenerateCode();
            return !string.IsNullOrWhiteSpace(code) ? $" {code}" : null;
        }
    }
}
