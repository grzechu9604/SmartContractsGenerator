using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGeneratorTests.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class IfStatementTests
    {
        private static readonly InstructionsListMockCreator instructionsListMockHelper = new InstructionsListMockCreator();
        private static readonly ConditionCodeMockCreator conditionMockHelper = new ConditionCodeMockCreator();

        private const string TrueInstructionsCode = "TRUE INSTRUCTIONS CODE";
        private const string FalseInstructionsCode = "FALSE INSTRUCTIONS CODE";
        private const string ConditionCode = "CONDITION CODE";
        private const string TrueInstructionsCode2 = "TRUE INSTRUCTIONS CODE2";
        private const string FalseInstructionsCode2 = "FALSE INSTRUCTIONS CODE2";
        private const string ConditionCode2 = "CONDITION CODE2";

        [TestCleanup]
        public void Cleanup()
        {
            instructionsListMockHelper.Dispose();
            conditionMockHelper.Dispose();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(IfStatement statement, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(statement != null);
            Assert.AreEqual(expected, statement.GenerateCode());
        }

        [TestMethod()]
        public void GenerateCodeElseIfFullTest()
        {
            var trueAndElseIfInsideIf = new IfStatement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode2),
                TrueInstructions = instructionsListMockHelper.PrepareMock(TrueInstructionsCode2, false, true),
                FalseInstructions = instructionsListMockHelper.PrepareMock(FalseInstructionsCode2, false, true)
            };

            var onlyIfInstructionsList = new InstructionsList();
            onlyIfInstructionsList.AppendInstruction(trueAndElseIfInsideIf);

            var trueAndElseIfFull = new IfStatement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode),
                TrueInstructions = instructionsListMockHelper.PrepareMock(TrueInstructionsCode, false, true),
                FalseInstructions = onlyIfInstructionsList
            };

            string expected = $"if ({ConditionCode}) {{\n{TrueInstructionsCode}\n}} else if ({ConditionCode2}) {{\n{TrueInstructionsCode2}\n}} else {{\n{FalseInstructionsCode2}\n}}";

            Assert.AreEqual(expected, trueAndElseIfFull.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            List<object[]> data = new List<object[]>();

            var onlyTrueInstructions = new IfStatement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode),
                TrueInstructions = instructionsListMockHelper.PrepareMock(TrueInstructionsCode, false, true),
                FalseInstructions = instructionsListMockHelper.PrepareMock(FalseInstructionsCode, false, false)
            };
            data.Add(new object[] { onlyTrueInstructions, $"if ({ConditionCode}) {{\n{TrueInstructionsCode}\n}}"});

            var trueAndFalseInstructions = new IfStatement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode),
                TrueInstructions = instructionsListMockHelper.PrepareMock(TrueInstructionsCode, false, true),
                FalseInstructions = instructionsListMockHelper.PrepareMock(FalseInstructionsCode, false, true)
            };
            data.Add(new object[] { trueAndFalseInstructions, $"if ({ConditionCode}) {{\n{TrueInstructionsCode}\n}} else {{\n{FalseInstructionsCode}\n}}" });

            var trueAndElseIfInstructions = new IfStatement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode),
                TrueInstructions = instructionsListMockHelper.PrepareMock(TrueInstructionsCode, false, true),
                FalseInstructions = instructionsListMockHelper.PrepareMock(FalseInstructionsCode, true, true)
            };
            data.Add(new object[] { trueAndElseIfInstructions, $"if ({ConditionCode}) {{\n{TrueInstructionsCode}\n}} else {FalseInstructionsCode}" });

            return data;
        }
    }
}