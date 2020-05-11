using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.Enums;
using System;

namespace SmartContractsGenerator.Model
{
    public class SpecialValueCall : ICodeGenerator, IInstruction
    {
        public BlockOrTransactionProperty? PropertyToCall { get; set; }
        
        public string GenerateCode()
        {
            if (PropertyToCall == null)
            {
                throw new MissingMandatoryElementException("PropertyToCall is obligatory element of SpecialValueCall");
            }

            return PropertyToCall.Value.GenerateCode();
        }
    }
}
