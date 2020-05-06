using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model.Enums;
using SmartContractsGeneratorTests.Model.Helpers;
using System;
using System.Collections.Generic;

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

            var returningType1 = "uint256";
            var returningType2 = "bool";

            var m1Name = "m1";
            var m1 = new Modifier()
            {
                Name = m1Name
            };

            var m2Name = "m2";
            var m2 = new Modifier()
            {
                Name = m2Name
            };

            var visibility1 = Visibility.External;
            var visibility2 = Visibility.Internal;
            var visibility3 = Visibility.Private;
            var visibility4 = Visibility.Public;

            var instructionCode1 = "INSTRUCTION CODE 1";
            var instructionCode2 = "INSTRUCTION CODE 2";

            var parametersCode1 = "PARAMETERS CODE 1";
            var parametersCode2 = "PARAMETERS CODE 2";

            var emptyFunctionExpected = $"function {name1}() {visibility1.GenerateCode()} {{\n}}";
            yield return GenerateRow(null, name1, null, visibility1, null, null, false, null, emptyFunctionExpected);
            yield return GenerateRow(string.Empty, name1, string.Empty, visibility1, null, null, false, null, emptyFunctionExpected);

            var expectedWithInstructions = $"function {name2}() {visibility2.GenerateCode()} {{\n{instructionCode1}\n}}";
            yield return GenerateRow(null, name2, instructionCode1, visibility2, null, null, false, null, expectedWithInstructions);
            yield return GenerateRow(string.Empty, name2, instructionCode1, visibility2, null, null, false, null, expectedWithInstructions);

            var expectedWithInstructionsAndParams = $"function {name2}({parametersCode1}) {visibility3.GenerateCode()} {{\n{instructionCode2}\n}}";
            yield return GenerateRow(parametersCode1, name2, instructionCode2, visibility3, null, null, false, null, expectedWithInstructionsAndParams);

            var expectedWithParams = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, null, null, false, null, expectedWithParams);

            var expectedWithModifier = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {m1.Name} {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, m1, null, false, null, expectedWithModifier);

            var expectedWithModifier2 = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {m2.Name} {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, m2, null, false, null, expectedWithModifier2);

            var expectedWithModifierAndEmptyParamList = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {m2.Name} {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, m2, parametersCode1, false, null, expectedWithModifierAndEmptyParamList);

            var expectedWithModifierAndEmptyParamList2 = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {m2.Name} {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, m2, parametersCode2, false, null, expectedWithModifierAndEmptyParamList2);

            var expectedWithModifierAndParamList = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {m2.Name}({parametersCode1}) {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, m2, parametersCode1, true, null, expectedWithModifierAndParamList);

            var expectedWithModifierAndParamList2 = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {m2.Name}({parametersCode2}) {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, m2, parametersCode2, true, null, expectedWithModifierAndParamList2);

            var expectedWithModifierAndReturns = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {m1.Name} returns ({returningType1}) {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, m1, null, false, returningType1, expectedWithModifierAndReturns);

            var expectedWithModifierAndReturns2 = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {m2.Name} returns ({returningType1}) {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, m2, null, false, returningType1, expectedWithModifierAndReturns2);

            var expectedWithModifierAndReturnsAndEmptyParamList = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {m2.Name} returns ({returningType1}) {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, m2, parametersCode1, false, returningType1, expectedWithModifierAndReturnsAndEmptyParamList);

            var expectedWithModifierAndReturnsAndEmptyParamList2 = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {m2.Name} returns ({returningType2}) {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, m2, parametersCode2, false, returningType2, expectedWithModifierAndReturnsAndEmptyParamList2);

            var expectedWithModifierAndReturnsAndParamList = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {m2.Name}({parametersCode1}) returns ({returningType2}) {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, m2, parametersCode1, true, returningType2, expectedWithModifierAndReturnsAndParamList);

            var expectedWithModifierAndReturnsAndParamList2 = $"function {name1}({parametersCode2}) {visibility4.GenerateCode()} {m2.Name}({parametersCode2}) returns ({returningType2}) {{\n}}";
            yield return GenerateRow(parametersCode2, name1, null, visibility4, m2, parametersCode2, true, returningType2, expectedWithModifierAndReturnsAndParamList2);
        }

        static object[] GenerateRow(string parametersListCode, string name, string instructionsListCode, Visibility? visibility, Modifier m, string modifierParametersListCode, bool anyModifierParameter, string returningType, string expected)
        {
            var parametersListMock = parametersListCode != null ? mockHelper.PrepareMock(parametersListCode, true) : null;
            var modifierParametersListMock = parametersListCode != null ? mockHelper.PrepareMock(modifierParametersListCode, anyModifierParameter) : null;
            var instructionsListMock = instructionsListCode != null ? instructionsListMockHelper.PrepareMock(instructionsListCode, false, !string.IsNullOrWhiteSpace(instructionsListCode)) : null;

            var f = new ContractFunction()
            {
                Instructions = instructionsListMock,
                Name = name,
                Parameters = parametersListMock,
                Visibility = visibility,
                Modifier = m,
                ModifierParameters = modifierParametersListMock,
                ReturningType = returningType
            };
            return new object[] { f, expected };
        }

        [TestMethod]
        [DynamicData(nameof(GenerateDataForGenerateCallCodeTest), DynamicDataSourceType.Method)]
        public void GenerateCallCodeTest(ContractFunction f, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(f != null);
            Assert.AreEqual(expected, f.GenerateCallCode());
        }


        static IEnumerable<object[]> GenerateDataForGenerateCallCodeTest()
        {
            var name1 = "Name1";
            yield return new object[] { new ContractFunction() { Name = name1 }, name1 };

            var name2 = "Name2";
            yield return new object[] { new ContractFunction() { Name = name2 }, name2 };
        }
    }
}