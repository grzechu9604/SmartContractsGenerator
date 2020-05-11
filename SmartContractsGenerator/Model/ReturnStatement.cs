using SmartContractsGenerator.Interfaces;

namespace SmartContractsGenerator.Model
{
    public class ReturnStatement : IOneLineInstruction
    {
        public IAssignable ToReturn { get; set; }

        public string GenerateCode()
        {
            if (ToReturn != null)
            {
                return $"return {ToReturn.GenerateCode()}";
            }
            else
            {
                return "return";
            }
        }
    }
}
