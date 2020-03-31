using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;

namespace SmartContractsGenerator.Model
{
    public class EventCall : AbstractCall
    {
        public ContractEvent EventToCall { get; set; }

        public override ICallable Callable => EventToCall;
        public override string CallingPrefix => "emit ";
    }
}
