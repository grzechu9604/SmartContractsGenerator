using SmartContractsGenerator.Model.AbstractPatterns;

namespace SmartContractsGenerator.Interfaces
{
    public interface ICodeGeneratorWithIndentation
    {
        string GenerateCode(Indentation indentation);
    }
}
