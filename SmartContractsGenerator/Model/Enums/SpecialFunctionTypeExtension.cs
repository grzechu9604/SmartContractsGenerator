using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmartContractsGenerator.Model.Enums
{
    public static class SpecialFunctionTypeExtension
    {
        private static readonly Dictionary<SpecialFunctionType, string> AppropriateCode = new Dictionary<SpecialFunctionType, string>()
        {
            { SpecialFunctionType.Receive, "receive" },
            { SpecialFunctionType.Fallback, "fallback" }
        };

        public static string GenerateCode(this SpecialFunctionType type)
        {
            return AppropriateCode[type];
        }
    }
}
