using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGeneratorTests.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

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
        public void GenerateCodeTest(InstructionsList instructions, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(instructions != null);
            Assert.AreEqual(expected, instructions.GenerateCode());
        }
        static IEnumerable<object[]> GetDataForTests()
        {
            var instructionCode1 = "INSTRUCTION CODE 1";
            var instructionCode2 = "INSTRUCTION CODE 2";
            var instructionCode3 = "INSTRUCTION CODE 3";
            var instructionCode4 = "INSTRUCTION CODE 4";

            var instructionMock1 = mockHelper.PrepareMock(instructionCode1);
            var instructionMock2 = mockHelper.PrepareMock(instructionCode2);
            var instructionMock3 = mockHelper.PrepareMock(instructionCode3);
            var instructionMock4 = mockHelper.PrepareMock(instructionCode4);

            var emptyInstructionsList = new InstructionsList();
            yield return new object[] { emptyInstructionsList, string.Empty };


            var instructionsList1 = new InstructionsList();
            instructionsList1.AppendInstruction(instructionMock1);
            yield return new object[] { instructionsList1, instructionCode1 };

            var instructionsList2 = new InstructionsList();
            instructionsList2.AppendInstruction(instructionMock1);
            instructionsList2.AppendInstruction(instructionMock2);
            yield return new object[] { instructionsList2, $"{instructionCode1}\n{instructionCode2}"};

            var instructionsList3 = new InstructionsList();
            instructionsList3.AppendInstruction(instructionMock1);
            instructionsList3.AppendInstruction(instructionMock2);
            instructionsList3.AppendInstruction(instructionMock3);
            yield return new object[] { instructionsList3, $"{instructionCode1}\n{instructionCode2}\n{instructionCode3}" };

            var instructionsList4 = new InstructionsList();
            instructionsList4.AppendInstruction(instructionMock1);
            instructionsList4.AppendInstruction(instructionMock2);
            instructionsList4.AppendInstruction(instructionMock3);
            instructionsList4.AppendInstruction(instructionMock4);
            yield return new object[] { instructionsList4, $"{instructionCode1}\n{instructionCode2}\n{instructionCode3}\n{instructionCode4}" };
        }
    }
}