using SmartContractsGenerator.Interfaces;
using System.Text;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public abstract class AbstractContainer : ICodeGenerable
    {
        public virtual string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GetHeader());
            sb.Append(GetContent());
            sb.Append(GetFooter());

            return sb.ToString();
        }

        protected abstract string GetHeader();
        protected abstract string GetContent();
        protected virtual string GetFooter() => "}";
    }
}
