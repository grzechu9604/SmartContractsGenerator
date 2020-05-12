using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Enums
{
    public static class SolidityTypeExtension
    {
        private static readonly Dictionary<SolidityType, string> AppropriateCode = new Dictionary<SolidityType, string>()
        {
            { SolidityType.Address, "address" },
            { SolidityType.AddressPayable, "address payable" },
            { SolidityType.Bool, "bool" },
            { SolidityType.Fixed, "fixed" },
            { SolidityType.Int, "int" },
            { SolidityType.String, "string" },
            { SolidityType.UFixed, "ufixed" },
            { SolidityType.UInt, "uint" }
        };


        public static string GenerateCode(this SolidityType type)
        {
            return AppropriateCode[type];
        }
    }
}
