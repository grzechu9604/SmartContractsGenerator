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
    public class AssignmentTests
    {
        private static readonly VariableMocksCreator variableMocksCreator = new VariableMocksCreator();

        [TestCleanup]
        public void Cleanup()
        {
            variableMocksCreator.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyAssignmentTest()
        {
            new Assignment().GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyAssignmentTest2()
        {
            new Assignment()
            {
                Destination = variableMocksCreator.PrepareMock("TEST", "TEST2", true)
            }.GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyAssignmentTest3()
        {
            new Assignment()
            {
                Source = variableMocksCreator.PrepareMock("TEST", "TEST2", true)
            }.GenerateCode();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForGenerateCodeTest), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(Assignment a, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(a != null);
            Assert.AreEqual(expected, a.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForGenerateCodeTest()
        {
            var v1 = "VARIABLE 1";
            var v2 = "VARIABLE 2";
            var a1 = new Assignment()
            {
                Destination = variableMocksCreator.PrepareMock(v1, null, false),
                Source = variableMocksCreator.PrepareMock(v2, null, false)
            };
            yield return new object[] { a1, $"{v1} = {v2}" };

            var a2 = new Assignment()
            {
                Destination = variableMocksCreator.PrepareMock(v2, null, false),
                Source = variableMocksCreator.PrepareMock(v1, null, false)
            };
            yield return new object[] { a2, $"{v2} = {v1}" };
        }
    }
}