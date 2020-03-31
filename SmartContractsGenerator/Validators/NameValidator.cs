using System.Text.RegularExpressions;

namespace SmartContractsGenerator.Validators
{
    public sealed class NameValidator
    {
        private static readonly Regex regex = new Regex(@"[a-zA-Z_]\w*");
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            else
            {
                var match = regex.Match(name);
                return match.Success && match.Length == name.Length && !IsInBannedWordsList(name);
            }
        }

        private static bool IsInBannedWordsList(string name)
        {
            //TODO Add solidity keywords list
            return false;
        }
    }
}
