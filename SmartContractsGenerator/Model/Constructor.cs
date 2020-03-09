using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Model.Enums;

namespace SmartContractsGenerator.Model
{
    public class Constructor : AbstractContainer
    {
        public Visibility? Visibility 
        { 
            get => visibility; 
            set
            {
                if (value == Enums.Visibility.Public || value == Enums.Visibility.Internal)
                {
                    visibility = value;
                }
                else
                {
                    throw new InvalidVisibilitySpecifierException("Constructor must be public or internal");
                }
            }
        }
        private Visibility? visibility;
        public ParametersList Parameters { get; set; }

        public InstructionsList Instructions { get; set; }

        protected override string GetContent()
        {
            return Instructions?.GenerateCode();
        }

        protected override string GetHeader() 
        {
            if (!Visibility.HasValue)
            {
                throw new MissingMandatoryElementException("Visibility specifier is required for constructor");
            }
            return $"constructor({Parameters?.GenerateCode()}) {Visibility.Value.GenerateCode()} {{\n";
        }
    }
}
