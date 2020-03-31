using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGeneratorTests.Model.Helpers;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class RequirementTests
    {
        private static readonly ConditionCodeMockCreator conditionMockHelper = new ConditionCodeMockCreator();
        private const string ConditionCode = "CONDITION CODE";

        [TestCleanup]
        public void Cleanup()
        {
            conditionMockHelper.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyRequirementTest()
        {
            new Requirement().GenerateCode();
        }

        [TestMethod()]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(Requirement r, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(r != null);
            Assert.AreEqual(expected, r.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            var r1 = new Requirement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode)
            };
            yield return new object[] { r1, $"require({ConditionCode});\n" };

            var r2 = new Requirement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode),
                ErrorMessage = string.Empty
            };
            yield return new object[] { r2, $"require({ConditionCode});\n" };

            string msg = "MSG";
            var r3 = new Requirement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode),
                ErrorMessage = msg
            };
            yield return new object[] { r3, $"require({ConditionCode}, {msg});\n" };
        }
    }
}