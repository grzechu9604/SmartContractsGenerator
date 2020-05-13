using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model.BuiltinFunctionCalls;
using SmartContractsGeneratorTests.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.BuiltinFunctionCalls.Tests
{
    [TestClass()]
    public class BalanceCallTests
    {
        private static readonly VariableMocksCreator mockHelper = new VariableMocksCreator();

        [TestCleanup]
        public void Cleanup()
        {
            mockHelper.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void MissingAllParamsTest()
        {
            new BalanceCall().GenerateCode();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]

        public void GenerateCodeTest(BalanceCall bc, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(bc != null);
            Assert.AreEqual(expected, bc.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            var addressCode1 = "ADDRESS CODE 1";
            var addressCode2 = "ADDRESS CODE 2";

            var addressMock1 = mockHelper.PrepareMock(addressCode1, null);
            var bc1 = new BalanceCall()
            {
                Address = addressMock1,
            };
            yield return new object[] { bc1, $"payable({addressCode1}).balance" };

            var addressMock2 = mockHelper.PrepareMock(addressCode2, null);
            var bc2 = new BalanceCall()
            {
                Address = addressMock2,
            };
            yield return new object[] { bc2, $"payable({addressCode2}).balance" };
        }
    }
}