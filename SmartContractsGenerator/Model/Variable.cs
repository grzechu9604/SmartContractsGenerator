using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.Enums;
using SmartContractsGenerator.Validators;
using System;

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
                    throw new InvalidOperationException($"Defined variable has invalid name - {value}");
                }
            }
        }
        private string name;

        public SolidityType? Type { get; set; }

        public virtual string GenerateDeclarationCode(bool pointStorageTypeForComplexObjects)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingMandatoryElementException("Name is required for Variable");
            }

            if (Type == null)
            {
                throw new MissingMandatoryElementException("Type is required for Variable");
            }

            return $"{Type.Value.GenerateCode(pointStorageTypeForComplexObjects)} {Name}";
        }

        public virtual string GenerateCode()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingMandatoryElementException("Name is required for Variable");
            }

            return $"{Name}";
        }
    }
}
