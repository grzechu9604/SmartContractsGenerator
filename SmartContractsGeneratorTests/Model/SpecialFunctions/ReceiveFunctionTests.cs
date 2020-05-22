using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGeneratorTests.Model.Helpers;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.SpecialFunctions.Tests
{
    [TestClass()]
    public class ReceiveFunctionTests
    {
        private static readonly InstructionsListMockCreator instructionsListMockHelper = new InstructionsListMockCreator();

        [TestCleanup]
        public void Cleanup()
        {
            instructionsListMockHelper.Dispose();
        }

        [TestMethod()]
        [DynamicData(nameof(GenerateTestData), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(ReceiveFunction rf, Indentation indentation, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(rf != null);
            Assert.AreEqual(expected, rf.GenerateCode(indentation));
        }

        static IEnumerable<object[]> GenerateTestData()
        {
            var instructionCode1 = "INSTRUCTION CODE 1";
            var instructionCode2 = "INSTRUCTION CODE 2";
            var indentation = new Indentation();

            var instructionsListMock = instructionsListMockHelper.PrepareMock(instructionCode1, false, true, indentation.GetIndentationWithIncrementedLevel());
            var instructionsListMock2 = instructionsListMockHelper.PrepareMock(instructionCode2, false, true, indentation.GetIndentationWithIncrementedLevel());
            var emptyInstructionsListMock = instructionsListMockHelper.PrepareMock(string.Empty, false, false, indentation.GetIndentationWithIncrementedLevel());

            yield return new object[] { new ReceiveFunction() { Instructions = instructionsListMock }, indentation, $"receive() external payable {{\n{instructionCode1}\n}}" };
            yield return new object[] { new ReceiveFunction() { Instructions = instructionsListMock2 }, indentation, $"receive() external payable {{\n{instructionCode2}\n}}" };
            yield return new object[] { new ReceiveFunction() { Instructions = emptyInstructionsListMock }, indentation, $"receive() external payable {{\n}}" };
        }
    }
}