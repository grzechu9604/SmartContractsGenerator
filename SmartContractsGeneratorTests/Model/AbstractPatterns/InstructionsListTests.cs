using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGeneratorTests.Model.Helpers;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.AbstractPatterns.Tests
{
    [TestClass()]
    public class InstructionsListTests
    {
        private static readonly InstructionMockHelper mockHelper = new InstructionMockHelper();

        [TestCleanup]
        public void Cleanup()
        {
            mockHelper.Dispose();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(InstructionsList instructions, Indentation indentation, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(instructions != null);
            Assert.AreEqual(expected, instructions.GenerateCode(indentation));
        }
        static IEnumerable<object[]> GetDataForTests()
        {
            var instructionCode1 = "INSTRUCTION CODE 1";
            var instructionCode2 = "INSTRUCTION CODE 2";
            var instructionCode3 = "INSTRUCTION CODE 3";
            var instructionCode4 = "INSTRUCTION CODE 4";

            var indentationLevelZero = new Indentation();

            var instructionMock1 = mockHelper.PrepareMock(instructionCode1, indentationLevelZero);
            var instructionMock2 = mockHelper.PrepareMock(instructionCode2, indentationLevelZero);
            var oneLineInstructionMock3 = mockHelper.PrepareOneLineInstructionMock(instructionCode3);
            var oneLineInstructionMock4 = mockHelper.PrepareOneLineInstructionMock(instructionCode4);

            var emptyInstructionsList = new InstructionsList();
            yield return new object[] { emptyInstructionsList, indentationLevelZero, string.Empty };

            yield return new object[] { emptyInstructionsList, indentationLevelZero.GetIndentationWithIncrementedLevel(), string.Empty };

            var instructionsList1 = new InstructionsList();
            instructionsList1.AppendInstruction(instructionMock1);
            yield return new object[] { instructionsList1, indentationLevelZero, instructionCode1 };

            var instructionMockWithLevel2 = mockHelper.PrepareMock(instructionCode1, indentationLevelZero.GetIndentationWithIncrementedLevel());
            var instructionsListWithLevel2 = new InstructionsList();
            instructionsListWithLevel2.AppendInstruction(instructionMockWithLevel2);
            yield return new object[] { instructionsListWithLevel2, indentationLevelZero.GetIndentationWithIncrementedLevel(), $"\t{instructionCode1}" };

            var oneLineInstructionsList1 = new InstructionsList();
            oneLineInstructionsList1.AppendInstruction(oneLineInstructionMock3);
            yield return new object[] { oneLineInstructionsList1, indentationLevelZero, $"{instructionCode3};" };

            var oneLineInstructionsList2 = new InstructionsList();
            oneLineInstructionsList2.AppendInstruction(oneLineInstructionMock3);
            oneLineInstructionsList2.AppendInstruction(oneLineInstructionMock4);
            yield return new object[] { oneLineInstructionsList2, indentationLevelZero.GetIndentationWithIncrementedLevel(), $"\t{instructionCode3};\n\t{instructionCode4};" };

            var instructionsList2 = new InstructionsList();
            instructionsList2.AppendInstruction(instructionMock1);
            instructionsList2.AppendInstruction(instructionMock2);
            yield return new object[] { instructionsList2, indentationLevelZero, $"{instructionCode1}\n{instructionCode2}"};

            var instructionsList3 = new InstructionsList();
            var instructionMockWithLevel3 = mockHelper.PrepareMock(instructionCode1, indentationLevelZero.GetIndentationWithIncrementedLevel().GetIndentationWithIncrementedLevel());
            var instructionMock2WithLevel3 = mockHelper.PrepareMock(instructionCode2, indentationLevelZero.GetIndentationWithIncrementedLevel().GetIndentationWithIncrementedLevel());

            instructionsList3.AppendInstruction(instructionMockWithLevel3);
            instructionsList3.AppendInstruction(instructionMock2WithLevel3);
            instructionsList3.AppendInstruction(oneLineInstructionMock3);
            var indentationLvl2 = indentationLevelZero.GetIndentationWithIncrementedLevel().GetIndentationWithIncrementedLevel();
            yield return new object[] { instructionsList3, indentationLvl2, $"\t\t{instructionCode1}\n\t\t{instructionCode2}\n\t\t{instructionCode3};" };

            var instructionsList4 = new InstructionsList();
            instructionsList4.AppendInstruction(instructionMock1);
            instructionsList4.AppendInstruction(instructionMock2);
            instructionsList4.AppendInstruction(oneLineInstructionMock3);
            instructionsList4.AppendInstruction(oneLineInstructionMock4);
            yield return new object[] { instructionsList4, indentationLevelZero, $"{instructionCode1}\n{instructionCode2}\n{instructionCode3};\n{instructionCode4};" };
        }
    }
}