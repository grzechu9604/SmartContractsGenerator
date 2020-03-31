using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class Contract : AbstractContainer
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
        public Constructor Constructor { get; set; }
        public List<ContractFunction> Functions { get; set; }
        public List<ContractEvent> Events { get; set; }
        public List<ContractProperty> Properties { get; set; }

        protected override string GetHeader()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingMandatoryElementException("Name is mandatory element of contract");
            }
            return $"contract {Name} {{\n";
        }

        protected override string GetContent()
        {
            StringBuilder codeBuilder = new StringBuilder();

            bool constructorAdded = false;
            if (Constructor != null)
            {
                codeBuilder.Append($"{Constructor.GenerateCode()}\n");
                constructorAdded = true;
            }

            bool propertiesAdded = false;
            if (Properties != null && Properties.Any())
            {
                if (constructorAdded)
                {
                    codeBuilder.Append("\n");
                }

                Properties.ForEach(property => codeBuilder.Append($"{property.GenerateDeclarationCode()}\n"));
                propertiesAdded = true;
            }

            bool eventsAdded = false;
            if (Events != null && Events.Any())
            {
                if (constructorAdded || propertiesAdded)
                {
                    codeBuilder.Append("\n");
                }

                Events.ForEach(contractEvent => codeBuilder.Append($"{contractEvent.GenerateCode()}\n"));
                eventsAdded = true;
            }

            if (Functions != null && Functions.Any())
            {
                if (constructorAdded || propertiesAdded || eventsAdded)
                {
                    codeBuilder.Append("\n");
                }

                Functions.Take(Functions.Count - 1).ToList().ForEach(function => codeBuilder.Append($"{function.GenerateCode()}\n\n"));

                codeBuilder.Append($"{Functions.Last().GenerateCode()}\n");
            }

            return codeBuilder.ToString();
        }
    }
}
