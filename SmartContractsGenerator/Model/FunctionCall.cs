using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;

namespace SmartContractsGenerator.Model
{
    public class FunctionCall : AbstractCall, IAssignable
    {
        public ContractFunction FunctionToCall { get; set; }

        public override ICallable Callable => FunctionToCall;
        public override string CallingPrefix => string.Empty;
    }
}
