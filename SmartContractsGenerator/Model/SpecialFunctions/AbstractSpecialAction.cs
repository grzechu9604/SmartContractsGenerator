using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Model.Enums;

namespace SmartContractsGenerator.Model.SpecialFunctions
{
    public abstract class AbstractSpecialAction : AbstractInstructionsContainer
    {
        public Visibility Visibility { get; } = Visibility.External;
        public abstract SpecialFunctionType Type { get; }
        protected abstract bool IsPayableFunction { get; }

        public string GetPayablePart() => IsPayableFunction ? " payable" : null;

        protected override string GetHeader() => $"{Type.GenerateCode()}() {Visibility.GenerateCode()}{GetPayablePart()} {{\n";
    }
}
