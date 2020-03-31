using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;

namespace SmartContractsGenerator.Model
{
    //TODO: function call should aslo have possibility to be assigned!!! - IAssignable interface
    public class Call : Instruction
    {
        public ICallable Callable { get; set; }
        public ParametersList Parameters { get; set; }

        public override string GenerateCode()
        {
            if (Callable == null)
            {
                throw new MissingMandatoryElementException("Element to call is mandatory element of Call");
            }

            return $"{Callable.GenerateCallCode()}({Parameters?.GenerateCallCode()})";
        }
    }
}
