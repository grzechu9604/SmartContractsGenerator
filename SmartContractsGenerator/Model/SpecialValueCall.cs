using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.Enums;

namespace SmartContractsGenerator.Model
{
    public class SpecialValueCall : IAssignable
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
