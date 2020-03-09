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
        public List<Declaration> Declarations { get; set; }

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

            if (Constructor != null)
            {
                codeBuilder.Append($"{Constructor.GenerateCode()}\n");
            }

            if (Declarations != null && Declarations.Any() )
            {
                if (Constructor != null)
                {
                    codeBuilder.Append("\n");
                }

                Declarations.ForEach(declaration => codeBuilder.Append($"{declaration.GenerateCode()}\n\n"));
            }

            return codeBuilder.ToString();
        }
    }
}
