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

        private static readonly HashSet<SolidityType> ComplexTypes = new HashSet<SolidityType>()
        {
            SolidityType.String
        };

        public static string GenerateCode(this SolidityType type, bool pointStorageTypeForComplexObjects)
        {
            return AppropriateCode[type] + (pointStorageTypeForComplexObjects ? GenerateStorageTypeCode(type) : string.Empty);
        }

        private static string GenerateStorageTypeCode(SolidityType type) => type.IsComplexType() ? " memory" : string.Empty;

        public static bool IsComplexType(this SolidityType type)
        {
            return ComplexTypes.Contains(type);
        }
    }
}
