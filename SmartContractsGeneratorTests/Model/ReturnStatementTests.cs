using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGeneratorTests.Model.Helpers;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ReturnStatementTests
    {
        private static readonly VariableMocksCreator variableMocksCreator = new VariableMocksCreator();

        [TestCleanup]
        public void Cleanup()
        {
            variableMocksCreator.Dispose();
        }

        [TestMethod()]
        public void EmptyAssignmentTest()
        {
            Assert.AreEqual("return", new ReturnStatement().GenerateCode());
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForGenerateCodeTest), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(ReturnStatement rs, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(rs != null);
            Assert.AreEqual(expected, rs.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForGenerateCodeTest()
        {
            var v1 = "VARIABLE 1";
            var v2 = "VARIABLE 2";

            var rs = new ReturnStatement()
            {
                ToReturn = variableMocksCreator.PrepareMock(v1, null, false)
            };
            yield return new object[] { rs, $"return {v1}" };

            var rs2 = new ReturnStatement()
            {
                ToReturn = variableMocksCreator.PrepareMock(v2, null, false)
            };
            yield return new object[] { rs2, $"return {v2}" };
        }
    }
}