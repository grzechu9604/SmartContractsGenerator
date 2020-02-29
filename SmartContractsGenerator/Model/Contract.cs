using SmartContractsGenerator.Model.AbstractPatterns;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class Contract : AbstractContainer
    {
        public string Name { get; set; }
        public Constructor Constructor { get; set; }
        public List<Declaration> Declarations { get; set; }

        protected override string GetHeader()
        {
            return $"contract {Name} {{\n";
        }

        protected override string GetContent()
        {
            StringBuilder codeBuilder = new StringBuilder();

            if (Constructor != null)
            {
                codeBuilder.AppendFormat("{0}\n\n", Constructor.GenerateCode());
            }

            Declarations?.ForEach(declaration => codeBuilder.AppendFormat("{0}\n\n", declaration.GenerateCode()));

            return codeBuilder.ToString();
        }

        protected override string GetFooter()
        {
            return "}";
        }
    }
}
