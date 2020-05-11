namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public abstract class AbstractInstructionsContainer : AbstractContainer
    {
        public InstructionsList Instructions { get; set; }

        protected override string GetContent(Indentation indentation)
        {
            if (Instructions != null && Instructions.Any())
            {
                return Instructions.GenerateCode(indentation) + '\n';
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
