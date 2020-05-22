using SmartContractsGenerator.Model.Enums;

namespace SmartContractsGenerator.Model.SpecialFunctions
{
    public class FallbackFunction : AbstractSpecialAction
    {
        public override SpecialFunctionType Type => SpecialFunctionType.Fallback;
        public bool IsPayable { get; set; }
        protected override bool IsPayableFunction => IsPayable;
    }
}
