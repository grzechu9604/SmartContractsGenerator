using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class Variable : IValueContainer, IAssignable
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

        public string Type { get; set; } // consider enum

        public string GenerateDeclarationCode()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingMandatoryElementException("Name is required for Variable");
            }

            if (string.IsNullOrWhiteSpace(Type))
            {
                throw new MissingMandatoryElementException("Type is required for Variable");
            }

            return $"{Type} {Name}";
        }

        public string GenerateCode()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingMandatoryElementException("Name is required for Variable");
            }

            return $"{Name}";
        }
    }
}
