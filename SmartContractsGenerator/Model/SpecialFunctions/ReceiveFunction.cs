using SmartContractsGenerator.Model.Enums;

namespace SmartContractsGenerator.Model.SpecialFunctions
{
    public class ReceiveFunction : AbstractSpecialAction
    {
        public override SpecialFunctionType Type => SpecialFunctionType.Receive;
        public bool IsPayable { get; } = true;
        protected override bool IsPayableFunction => IsPayable;
    }
}
