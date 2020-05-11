using SmartContractsGenerator.Interfaces;

namespace SmartContractsGenerator.Model
{
    public class BreakStatement : IInstruction, ICodeGenerator
    {
        public string GenerateCode() => "break";
    }
}
