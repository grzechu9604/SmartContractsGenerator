using SmartContractsGenerator.Model.AbstractPatterns;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class Contract : AbstractContainer
    {
        public string Name { get; set; }
        public Constructor Constructor { get; set; }
        public List<Declaration> Declarations { get; set; }

        protected override string GetHeader() => $"contract {Name} {{\n";

        protected override string GetContent()
        {
            StringBuilder codeBuilder = new StringBuilder();

            if (Constructor != null)
            {
                codeBuilder.AppendFormat("{0}\n", Constructor.GenerateCode());
            }

            if (Declarations != null && Declarations.Any() )
            {
                if (Constructor != null)
                {
                    codeBuilder.Append("\n");
                }

                Declarations.ForEach(declaration => codeBuilder.AppendFormat("{0}\n\n", declaration.GenerateCode()));
            }

            return codeBuilder.ToString();
        }

        protected override string GetFooter() => "}";
    }
}
