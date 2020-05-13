using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGeneratorTests.Model.Helpers;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.SpecialFunctions.Tests
{
    [TestClass()]
    public class FallbackFunctionTests
    {
        private static readonly InstructionsListMockCreator instructionsListMockHelper = new InstructionsListMockCreator();

        [TestCleanup]
        public void Cleanup()
        {
            instructionsListMockHelper.Dispose();
        }

        [TestMethod()]
        [DynamicData(nameof(GenerateTestData), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(FallbackFunction ff, Indentation indentation, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(ff != null);
            Assert.AreEqual(expected, ff.GenerateCode(indentation));
        }

        static IEnumerable<object[]> GenerateTestData()
        {
            var instructionCode1 = "INSTRUCTION CODE 1";
            var instructionCode2 = "INSTRUCTION CODE 2";
            var indentation = new Indentation();

            var instructionsListMock = instructionsListMockHelper.PrepareMock(instructionCode1, false, true, indentation.GetIndentationWithIncrementedLevel());
            var instructionsListMock2 = instructionsListMockHelper.PrepareMock(instructionCode2, false, true, indentation.GetIndentationWithIncrementedLevel());
            var emptyInstructionsListMock = instructionsListMockHelper.PrepareMock(string.Empty, false, false, indentation.GetIndentationWithIncrementedLevel());

            yield return new object[] { new FallbackFunction() { Instructions = instructionsListMock, IsPayable = true }, indentation, $"fallback() external payable {{\n{instructionCode1}\n}}" };
            yield return new object[] { new FallbackFunction() { Instructions = instructionsListMock2, IsPayable = true }, indentation, $"fallback() external payable {{\n{instructionCode2}\n}}" };
            yield return new object[] { new FallbackFunction() { Instructions = emptyInstructionsListMock, IsPayable = true }, indentation, $"fallback() external payable {{\n}}" };
            yield return new object[] { new FallbackFunction() { Instructions = instructionsListMock, IsPayable = false }, indentation, $"fallback() external {{\n{instructionCode1}\n}}" };
            yield return new object[] { new FallbackFunction() { Instructions = instructionsListMock2, IsPayable = false }, indentation, $"fallback() external {{\n{instructionCode2}\n}}" };
            yield return new object[] { new FallbackFunction() { Instructions = emptyInstructionsListMock, IsPayable = false }, indentation, $"fallback() external {{\n}}" };
        }
    }
}