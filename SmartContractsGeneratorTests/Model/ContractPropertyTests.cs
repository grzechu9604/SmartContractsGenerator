using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model.Enums;
using SmartContractsGeneratorTests.Model.Helpers;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ContractPropertyTests
    {
        private static readonly VariableMocksCreator mocksCreator = new VariableMocksCreator();

        [TestCleanup]
        public void Cleanup()
        {
            mocksCreator.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyContractPropertyTest()
        {
            new ContractProperty().GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyContractPropertyTest2()
        {
            new ContractProperty().GenerateDeclarationCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyContractPropertyTest3()
        {
            new ContractProperty()
            {
                Visibility = Enums.Visibility.External
            }.GenerateDeclarationCode();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForGenerateCodeTest), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(ContractProperty cp, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(cp != null);
            Assert.AreEqual(expected, cp.GenerateCode());
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForGenerateDeclarationCodeTest), DynamicDataSourceType.Method)]
        public void GenerateDeclarationCodeTest(ContractProperty cp, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(cp != null);
            Assert.AreEqual(expected, cp.GenerateDeclarationCode());
        }

        static IEnumerable<object[]> GetDataForGenerateCodeTest()
        {
            string expectedCode1 = "EXPECTED CODE 1";
            var m1 = GetVariableMock(expectedCode1, null);
            var p1 = new ContractProperty()
            {
                Variable = m1,
                Visibility = Enums.Visibility.External
            };
            yield return new object[] { p1, $"{expectedCode1}" };

            string expectedCode2 = "EXPECTED CODE 2";
            var m2 = GetVariableMock(expectedCode2, null);
            var p2 = new ContractProperty()
            {
                Variable = m2,
                Visibility = Enums.Visibility.External
            };
            yield return new object[] { p2, $"{expectedCode2}" };

            string expectedCode3 = "EXPECTED CODE 3";
            var m3 = GetVariableMock(expectedCode3, null);
            var p3 = new ContractProperty()
            {
                Variable = m3,
                Visibility = Enums.Visibility.External
            };
            yield return new object[] { p3, $"{expectedCode3}" };
        }

        static IEnumerable<object[]> GetDataForGenerateDeclarationCodeTest()
        {
            string type1 = "TYPE1";
            string name1 = "NAME1";
            Visibility v1 = Visibility.Public;
            yield return new object[] { GetContractProperty(type1, name1, v1), $"{type1} {v1.GenerateCode()} {name1}" };

            string type2 = "TYPE2";
            string name2 = "NAME2";
            Visibility v2 = Visibility.Private;
            yield return new object[] { GetContractProperty(type2, name2, v2), $"{type2} {v2.GenerateCode()} {name2}" };

            string type3 = "TYPE3";
            string name3 = "NAME3";
            Visibility v3 = Visibility.Private;
            yield return new object[] { GetContractProperty(type3, name3, v3), $"{type3} {v3.GenerateCode()} {name3}" };
        }

        private static Variable GetVariableMock(string expectedCode, string expectedDeclarationCode)
        {
            return mocksCreator.PrepareMock(expectedCode, expectedDeclarationCode);
        }

        private static ContractProperty GetContractProperty(string vType, string vName, Visibility visibility)
        {
            var v = new Variable()
            {
                Name = vName,
                Type = vType
            };

            return new ContractProperty()
            {
                Variable = v,
                Visibility = visibility
            };
        }
    }
}