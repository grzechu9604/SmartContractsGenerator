using SmartContractsGenerator.Interfaces;
using System.Text;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public abstract class AbstractContainer : ICodeGeneratorWithIndentation
    {
        public virtual string GenerateCode(Indentation indentation)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{GetHeader()}"); // indentation in header line must be set by parent
            sb.Append(GetContent(indentation?.GetIndentationWithIncrementedLevel()));
            sb.Append($"{GetFooter(indentation)}");

            return sb.ToString();
        }

        protected abstract string GetHeader();
        protected abstract string GetContent(Indentation indentation);
        protected string GetFooter(Indentation indentation)
        {
            var sb = new StringBuilder();

            var prefix = GetFooterPrefix();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                sb.Append($"{indentation?.GetIndentationWithIncrementedLevel().GenerateCode()}{GetFooterPrefix()}\n");
            }

            sb.Append($"{indentation?.GenerateCode()}}}");

            return sb.ToString();
        }

        protected virtual string GetFooterPrefix() => null;
    }
}
