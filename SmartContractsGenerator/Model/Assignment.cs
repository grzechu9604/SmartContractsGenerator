using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;

namespace SmartContractsGenerator.Model
{
    public class Assignment : IOneLineInstruction
    {
        public IValueContainer Destination { get; set; }
        public IAssignable Source { get; set; }

        public virtual string GenerateCode()
        {
            if (Destination == null)
            {
                throw new MissingMandatoryElementException("Destination is required in assignment!");
            }

            if (Source == null)
            {
                throw new MissingMandatoryElementException("Source is required in assignment!");
            }

            return $"{Destination.GenerateCode()} = {Source.GenerateCode()}";
        }
    }
}
