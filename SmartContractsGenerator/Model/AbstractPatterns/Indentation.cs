namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public class Indentation
    {
        public Indentation()
        {
            IndentationLevel = 0;
        }

        private Indentation(int indentationLevel)
        {
            IndentationLevel = indentationLevel;
        }

        private readonly int IndentationLevel;

        public Indentation GetIndentationWithIncrementedLevel() => new Indentation(IndentationLevel + 1);

        public string GenerateCode()
        {
            return new string('\t', IndentationLevel);
        }
    }
}
