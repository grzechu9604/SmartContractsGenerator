using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Enums.Tests
{
    [TestClass()]
    public class SolidityTypeExtensionTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(SolidityType type, string expected)
        {
            Assert.AreEqual(expected, type.GenerateCode(false));
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            var appropriateCode = new Dictionary<SolidityType, string>()
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

            foreach (var entry in appropriateCode)
            {
                yield return new object[] { entry.Key, entry.Value };
            }
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForWithMemoryTests), DynamicDataSourceType.Method)]
        public void GenerateCodeWithMemoryTest(SolidityType type, string expected)
        {
            Assert.AreEqual(expected, type.GenerateCode(true));
        }

        static IEnumerable<object[]> GetDataForWithMemoryTests()
        {
            var appropriateCode = new Dictionary<SolidityType, string>()
            {
                { SolidityType.Address, "address" },
                { SolidityType.AddressPayable, "address payable" },
                { SolidityType.Bool, "bool" },
                { SolidityType.Fixed, "fixed" },
                { SolidityType.Int, "int" },
                { SolidityType.String, "string memory" },
                { SolidityType.UFixed, "ufixed" },
                { SolidityType.UInt, "uint" }
            };

            foreach (var entry in appropriateCode)
            {
                yield return new object[] { entry.Key, entry.Value };
            }
        }
    }
}