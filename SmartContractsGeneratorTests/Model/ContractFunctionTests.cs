using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.Enums;
using SmartContractsGeneratorTests.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ContractFunctionTests
    {
        private static readonly ParametersListMockCreator mockHelper = new ParametersListMockCreator();
        private static readonly InstructionsListMockCreator instructionsListMockHelper = new InstructionsListMockCreator();

        [TestCleanup]
        public void Cleanup()
        {
            mockHelper.Dispose();
            instructionsListMockHelper.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidContractNameTest()
        {
            var function = new ContractFunction()
            {
                Name = "123"
            };
            function.GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyContractNameTest()
        {
            var function = new ContractFunction();
            function.GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyContractVisibilityTest()
        {
            var function = new ContractFunction()
            {
                Name = "Test"
            };
            function.GenerateCode();
        }

        [TestMethod]
        [DynamicData(nameof(GenerateTestData), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(ContractFunction function, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(function != null);
            Assert.AreEqual(expected, function.GenerateCode());
        }

        static IEnumerable<object[]> GenerateTestData()
        {
            var name1 = "Name1";
            var name2 = "Name2";

            var visibility1 = Visibility.External;
            var visibility2 = Visibility.Internal;
            var visibility3 = Visibility.Private;
            var visibility4 = Visibility.Public;

            var instructionCode1 = "INSTRUCTION CODE 1";
            var instructionCode2 = "INSTRUCTION CODE 2";

            var parametersCode1 = "PARAMETERS CODE 1";
            var parametersCode2 = "PARAMETERS CODE 2";

            var emptyFunctionExpected = $"function {name1}() {visibility1.GenerateCode()} {{\n}}";
            yield return GenerateRow(null, name1, null, visibility1, emptyFunctionExpected);
            yield return GenerateRow(string.Empty, name1, string.Empty, visibility1, emptyFunctionExpected);

            var expectedWithInstructions = $"function {name2}() {visibility2.GenerateCode()} {{\n{instructionCode1}\n}}";
            yield return GenerateRow(null, name2, instructionCode1, visibility2, expectedWithInstructions);
            yield return GenerateRow(string.Empty, name2, instructionCode1, visibility2, expectedWithInstructions);

            var expectedWithInstructionsAndParams = $"function {name2}({parametersCode1}) {visibility3.GenerateCode()} {{\n{instructionCode2}\n}}";
            yield return GenerateRow(parametersCode1, name2, instructionCode2, visibility3, expectedWithInstructionsAndParams);

            var expectedWithParams = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, expectedWithParams);
        }

        static object[] GenerateRow(string parametersListCode, string name, string instructionsListCode, Visibility? visibility , string expected)
        {
            var parametersListMock = parametersListCode != null ? mockHelper.PrepareMock(parametersListCode) : null;
            var instructionsListMock = instructionsListCode != null ? instructionsListMockHelper.PrepareMock(instructionsListCode, false, !string.IsNullOrWhiteSpace(instructionsListCode)) : null;

            var f = new ContractFunction()
            {
                Instructions = instructionsListMock,
                Name = name,
                Parameters = parametersListMock,
                Visibility = visibility
            };
            return new object[] { f, expected };
        }
    }
}