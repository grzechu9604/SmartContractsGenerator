using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public abstract class AbstractCall : IOneLineInstruction
    {
        public abstract ICallable Callable { get; }
        public abstract string CallingPrefix { get; }
        public CallingParametersList Parameters { get; set; }

        public virtual string GenerateCode()
        {
            if (Callable == null)
            {
                throw new MissingMandatoryElementException("Element to call is mandatory element of Call");
            }

            return $"{CallingPrefix}{Callable.GenerateCallCode()}({Parameters?.GenerateCode()})";
        }
    }
}
