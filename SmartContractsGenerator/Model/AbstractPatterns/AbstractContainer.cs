using SmartContractsGenerator.Interfaces;
using System.Text;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public abstract class AbstractContainer : ICodeGenerable
    {
        public string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GetHeader());
            sb.Append(GetContent());
            sb.Append(GetFooter());

            return sb.ToString();
        }

        protected abstract string GetHeader();
        protected abstract string GetContent();
        protected abstract string GetFooter();
    }
}
