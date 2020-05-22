using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;

namespace SmartContractsGenerator.Model.BuiltinFunctionCalls
{
    public class TransferCall : IOneLineInstruction, ICodeGenerator
    {
        public IAssignable Address { get; set; }
        public IAssignable ValueToTransfer { get; set; }

        public string GenerateCode()
        {
            if (Address == null)
            {
                throw new MissingMandatoryElementException("Address is obligatory element of Transfer function call");
            }

            if (ValueToTransfer == null)
            {
                throw new MissingMandatoryElementException("Address is obligatory element of Transfer function call");
            }

            return $"payable({Address.GenerateCode()}).transfer({ValueToTransfer.GenerateCode()})";
        }
    }
}
