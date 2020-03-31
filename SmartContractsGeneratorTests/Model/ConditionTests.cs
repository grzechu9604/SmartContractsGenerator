using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model;
using SmartContractsGeneratorTests.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ConditionTests
    {
        private static readonly OperationMockCreator mockCreator = new OperationMockCreator();

        [TestCleanup]

        public void Cleanup()
        {
            mockCreator.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyConditionTest()
        {
            new Condition().GenerateCode();
        }

        [TestMethod()]
        [DynamicData(nameof(GetDataForGenerateCodeTest), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(Condition c, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(c != null);
            Assert.AreEqual(expected, c.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForGenerateCodeTest()
        {
            var operation1 = "OPERATION 1";
            yield return GenerateRow(operation1, operation1);

            var operation2 = "OPERATION 2";
            yield return GenerateRow(operation2, operation2);

            var operation3 = "OPERATION 3";
            yield return GenerateRow(operation3, operation3);
        }

        static object[] GenerateRow(string operationExpectedCode, string expected)
        {
            var operationMock = mockCreator.PrepareMock(operationExpectedCode);

            var c = new Condition()
            {
                ConditionOperation = operationMock
            };
            return new object[] { c, expected };
        }
    }
}