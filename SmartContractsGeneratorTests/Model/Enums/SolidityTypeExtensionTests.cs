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
            Assert.AreEqual(expected, type.GenerateCode());
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
    }
}