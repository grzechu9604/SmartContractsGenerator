using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGeneratorTests.Model.Helpers;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class DeclarationTests
    {
        private static readonly VariableMocksCreator variableMocksCreator = new VariableMocksCreator();

        [TestCleanup]
        public void Cleanup()
        {
            variableMocksCreator.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyDeclarationTest()
        {
            new Declaration().GenerateCode();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForGenerateCodeTest), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(Declaration d, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(d != null);
            Assert.AreEqual(expected, d.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForGenerateCodeTest()
        {
            var d1 = "DECLARATION 1";
            yield return new object[] { new Declaration() { Variable = variableMocksCreator.PrepareMock(null, d1) }, d1 };
            var d2 = "DECLARATION 2";
            yield return new object[] { new Declaration() { Variable = variableMocksCreator.PrepareMock(null, d2) }, d2 };
        }
    }
}