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
    public class TransferCallTests
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
            new TransferCall().GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void MissingValueToTransferTest()
        {
            new TransferCall()
            {
                Address = new ConstantValue()
            }.GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void MissingAddressTest()
        {
            new TransferCall()
            {
                ValueToTransfer = new ConstantValue()
            }.GenerateCode();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(TransferCall tc, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(tc != null);
            Assert.AreEqual(expected, tc.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            var addressCode1 = "ADDRESS CODE 1";
            var addressCode2 = "ADDRESS CODE 2";
            var valueCode1 = "VALUE CODE 1";
            var valueCode2 = "VALUE CODE 2";

            var addressMock1 = mockHelper.PrepareMock(addressCode1, null);
            var valueMock1 = mockHelper.PrepareMock(valueCode1, null);
            var tc1 = new TransferCall()
            {
                Address = addressMock1,
                ValueToTransfer = valueMock1
            };
            yield return new object[] { tc1, $"payable({addressCode1}).transfer({valueCode1})" };

            var addressMock2 = mockHelper.PrepareMock(addressCode2, null);
            var valueMock2 = mockHelper.PrepareMock(valueCode2, null);
            var tc2 = new TransferCall()
            {
                Address = addressMock2,
                ValueToTransfer = valueMock2
            };
            yield return new object[] { tc2, $"payable({addressCode2}).transfer({valueCode2})" };
        }
    }
}