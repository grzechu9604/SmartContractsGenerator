namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public class Indentation
    {
        private int IndentationLevel = 0;

        public void IncrementIndentationLevel() => IndentationLevel++;

        public string GenerateCode()
        {
            return new string('\t', IndentationLevel);
        }
    }
}
