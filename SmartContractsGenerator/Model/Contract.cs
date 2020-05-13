using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Model.SpecialFunctions;
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
        public FallbackFunction FallbackFunction { get; set; }
        public ReceiveFunction ReceiveFunction { get; set; }
        public List<ContractFunction> Functions { get; set; }
        public List<ContractEvent> Events { get; set; }
        public List<ContractProperty> Properties { get; set; }
        public List<Modifier> Modifiers { get; set; }

        protected override string GetHeader()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingMandatoryElementException("Name is mandatory element of contract");
            }
            return $"contract {Name} {{\n";
        }

        protected override string GetContent(Indentation indentation)
        {
            StringBuilder codeBuilder = new StringBuilder();

            bool applyAdditionalBlankLine = false;
            if (Properties != null && Properties.Any())
            {
                Properties.ForEach(property => codeBuilder.Append($"{indentation?.GenerateCode()}{property.GenerateDeclarationCode()};\n"));
                applyAdditionalBlankLine = true;
            }

            if (Events != null && Events.Any())
            {
                if (applyAdditionalBlankLine)
                {
                    codeBuilder.Append("\n");
                }
                applyAdditionalBlankLine = true;

                Events.ForEach(contractEvent => codeBuilder.Append($"{indentation?.GenerateCode()}{contractEvent.GenerateCode()};\n"));
            }

            if (Constructor != null)
            {
                if (applyAdditionalBlankLine)
                {
                    codeBuilder.Append("\n");
                }
                applyAdditionalBlankLine = true;

                codeBuilder.Append($"{indentation?.GenerateCode()}{Constructor.GenerateCode(indentation)}\n");
            }

            if (FallbackFunction != null)
            {
                if (applyAdditionalBlankLine)
                {
                    codeBuilder.Append("\n");
                }
                applyAdditionalBlankLine = true;

                codeBuilder.Append($"{indentation?.GenerateCode()}{FallbackFunction.GenerateCode(indentation)}\n");
            }

            if (ReceiveFunction != null)
            {
                if (applyAdditionalBlankLine)
                {
                    codeBuilder.Append("\n");
                }
                applyAdditionalBlankLine = true;

                codeBuilder.Append($"{indentation?.GenerateCode()}{ReceiveFunction.GenerateCode(indentation)}\n");
            }

            if (Modifiers != null && Modifiers.Any())
            {
                if (applyAdditionalBlankLine)
                {
                    codeBuilder.Append("\n");
                }
                applyAdditionalBlankLine = true;

                Modifiers.Take(Modifiers.Count - 1).ToList().ForEach(modifer => 
                    codeBuilder.Append($"{indentation?.GenerateCode()}{modifer.GenerateCode(indentation)}\n\n"));

                codeBuilder.Append($"{indentation?.GenerateCode()}{Modifiers.Last().GenerateCode(indentation)}\n");
            }

            if (Functions != null && Functions.Any())
            {
                if (applyAdditionalBlankLine)
                {
                    codeBuilder.Append("\n");
                }

                Functions.Take(Functions.Count - 1).ToList().ForEach(function => 
                    codeBuilder.Append($"{indentation?.GenerateCode()}{function.GenerateCode(indentation)}\n\n"));

                codeBuilder.Append($"{indentation?.GenerateCode()}{Functions.Last().GenerateCode(indentation)}\n");
            }

            return codeBuilder.ToString();
        }
    }
}
