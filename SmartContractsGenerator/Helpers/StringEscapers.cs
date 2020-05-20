using System.Text.RegularExpressions;

namespace SmartContractsGenerator.Helpers
{
    public static class SolidityStringsEscaper
    {
        private static readonly char[] CharsToEscape = new char[] { '\\', '"' };

        public static string EscapeString(string toEscape)
        {
            return $"\"{Regex.Replace(toEscape, $"[{string.Join('\\', CharsToEscape)}]", m => $"\\{m.Value}")}\"";
        }
    }
}
