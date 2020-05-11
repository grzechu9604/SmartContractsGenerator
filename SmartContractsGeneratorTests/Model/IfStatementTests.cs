using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGeneratorTests.Model.Helpers;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class IfStatementTests
    {
        private static readonly InstructionsListMockCreator instructionsListMockHelper = new InstructionsListMockCreator();
        private static readonly ConditionCodeMockCreator conditionMockHelper = new ConditionCodeMockCreator();

        private const string ConditionCode = "CONDITION CODE";
        private const string ConditionCode2 = "CONDITION CODE2";

        [TestCleanup]
        public void Cleanup()
        {
            instructionsListMockHelper.Dispose();
            conditionMockHelper.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyIfStatementTest()
        {
            new IfStatement().GenerateCode(new Indentation());
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(IfStatement statement, Indentation indentation, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(statement != null);
            Assert.AreEqual(expected, statement.GenerateCode(indentation));
        }

        [TestMethod()]
        public void GenerateCodeElseIfFullTest()
        {
            var trueInstructionsCode = "\tTRUE INSTRUCTIONS CODE";
            var trueInstructionsCode2 = "\tTRUE INSTRUCTIONS CODE2";
            var falseInstructionsCode = "\tFALSE INSTRUCTIONS CODE";

            var indentation = new Indentation();

            var trueAndElseIfInsideIf = new IfStatement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode2),
                TrueInstructions = instructionsListMockHelper.PrepareMock(trueInstructionsCode2, false, true, indentation.GetIndentationWithIncrementedLevel()),
                FalseInstructions = instructionsListMockHelper.PrepareMock(falseInstructionsCode, false, true, indentation.GetIndentationWithIncrementedLevel())
            };

            var onlyIfInstructionsList = new InstructionsList();
            onlyIfInstructionsList.AppendInstruction(trueAndElseIfInsideIf);

            var trueAndElseIfFull = new IfStatement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode),
                TrueInstructions = instructionsListMockHelper.PrepareMock(trueInstructionsCode, false, true, indentation.GetIndentationWithIncrementedLevel()),
                FalseInstructions = onlyIfInstructionsList
            };

            string expected = $"if ({ConditionCode}) {{\n{trueInstructionsCode}\n}} else if ({ConditionCode2}) {{\n{trueInstructionsCode2}\n}} else {{\n{falseInstructionsCode}\n}}";

            Assert.AreEqual(expected, trueAndElseIfFull.GenerateCode(indentation));
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            var indentation = new Indentation();

            var trueInstructionsCode = "\tTRUE INSTRUCTIONS CODE";
            var falseInstructionsCode = "\tFALSE INSTRUCTIONS CODE";

            var onlyTrueInstructions = new IfStatement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode),
                TrueInstructions = instructionsListMockHelper.PrepareMock(trueInstructionsCode, false, true, indentation.GetIndentationWithIncrementedLevel()),
                FalseInstructions = instructionsListMockHelper.PrepareMock(falseInstructionsCode, false, false, indentation.GetIndentationWithIncrementedLevel())
            };
            yield return new object[] { onlyTrueInstructions, indentation, $"if ({ConditionCode}) {{\n{trueInstructionsCode}\n}}" };

            trueInstructionsCode = "\t\tTRUE INSTRUCTIONS CODE";

            var onlyTrueInstructionsDoubleIndentation = new IfStatement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode),
                TrueInstructions = instructionsListMockHelper.PrepareMock(trueInstructionsCode, false, true, indentation.GetIndentationWithIncrementedLevel().GetIndentationWithIncrementedLevel()),
                FalseInstructions = instructionsListMockHelper.PrepareMock(falseInstructionsCode, false, false, indentation.GetIndentationWithIncrementedLevel().GetIndentationWithIncrementedLevel())
            };
            yield return new object[] { onlyTrueInstructionsDoubleIndentation, indentation.GetIndentationWithIncrementedLevel(), $"if ({ConditionCode}) {{\n{trueInstructionsCode}\n\t}}" };

            trueInstructionsCode = "\tTRUE INSTRUCTIONS CODE";
            falseInstructionsCode = "\tFALSE INSTRUCTIONS CODE";

            var trueAndFalseInstructions = new IfStatement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode),
                TrueInstructions = instructionsListMockHelper.PrepareMock(trueInstructionsCode, false, true, indentation.GetIndentationWithIncrementedLevel()),
                FalseInstructions = instructionsListMockHelper.PrepareMock(falseInstructionsCode, false, true, indentation.GetIndentationWithIncrementedLevel())
            };
            yield return new object[] { trueAndFalseInstructions, indentation, $"if ({ConditionCode}) {{\n{trueInstructionsCode}\n}} else {{\n{falseInstructionsCode}\n}}" };


            trueInstructionsCode = "\t\tTRUE INSTRUCTIONS CODE";
            falseInstructionsCode = "\t\tFALSE INSTRUCTIONS CODE";

            var trueAndFalseInstructionsDoubleIndentation = new IfStatement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode),
                TrueInstructions = instructionsListMockHelper.PrepareMock(trueInstructionsCode, false, true, indentation.GetIndentationWithIncrementedLevel().GetIndentationWithIncrementedLevel()),
                FalseInstructions = instructionsListMockHelper.PrepareMock(falseInstructionsCode, false, true, indentation.GetIndentationWithIncrementedLevel().GetIndentationWithIncrementedLevel())
            };
            yield return new object[] { trueAndFalseInstructionsDoubleIndentation, indentation.GetIndentationWithIncrementedLevel(), $"if ({ConditionCode}) {{\n{trueInstructionsCode}\n\t}} else {{\n{falseInstructionsCode}\n\t}}" };


            var onlyCondition = new IfStatement()
            {
                Condition = conditionMockHelper.PrepareMock(ConditionCode)
            };
            yield return new object[] { onlyCondition, indentation, $"if ({ConditionCode}) {{\n}}" };
        }
    }
}