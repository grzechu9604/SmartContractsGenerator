using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class Assignment : Instruction
    {
        public IValueContainer Destination { get; set; }
        public IAssignable Source { get; set; }

        public override string GenerateCode()
        {
            if (Destination == null)
            {
                throw new MissingMandatoryElementException("Destination is required in assignment!");
            }

            if (Source == null)
            {
                throw new MissingMandatoryElementException("Source is required in assignment!");
            }

            return $"{Destination.GenerateCode()} = {Source.GenerateCode()};";
        }
    }
}
