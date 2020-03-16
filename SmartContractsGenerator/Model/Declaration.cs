using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class Declaration : IValueContainer
    {
        public Variable Variable { get; set; }
        public string GenerateCode()
        {
            if (Variable == null)
            {
                throw new MissingMandatoryElementException("Variable is required for Declaration");
            }

            return Variable.GenerateDeclarationCode();
        }
    }
}
