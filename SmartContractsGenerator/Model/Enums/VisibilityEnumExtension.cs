using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.Enums
{
    public static class VisibilityEnumExtension
    {
        public static string GenerateCode(this Visibility visibility)
        {
            switch (visibility)
            {
                case Visibility.External:
                    return "external";
                case Visibility.Public:
                    return "public";
                case Visibility.Internal:
                    return "internal";
                case Visibility.Private:
                    return "private";
            }

            throw new InvalidOperationException("Visibility has invalid value");
        }
    }
}
