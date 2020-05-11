using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;

namespace SmartContractsGenerator.Model
{
    public class ModifierAppliance : AbstractCall
    {
        public override ICallable Callable => ModifierToApply;

        public Modifier ModifierToApply { get; set; }

        public override string CallingPrefix => null;
    }
}
