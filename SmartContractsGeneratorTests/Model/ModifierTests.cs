using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGeneratorTests.Model.Helpers;
using System;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ModifierTests
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
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyModifierNameTest()
        {
            new Modifier().GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidModifierNameTest()
        {
            new Modifier()
            {
                Name = "123"
            };
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void ConstructorTest(Modifier m, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(m != null);
            Assert.AreEqual(expected, m.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            string modifierName1 = "Name";
            string modifierName2 = "Name2";
            var oneElementParametersListCode = "Type1 Name1";
            var twoElementParametersListCode = "Type1 Name1, Type2 Name2";
            var threeElementParametersListCode = "Type1 Name1, Type2 Name2, Type3 Name3";
            var instructionsCode1 = "INSTRUCTIONS CODE 1";
            var instructionsCode2 = "INSTRUCTIONS CODE 2";

            yield return GenerateRow(null, modifierName1, null, $"modifier {modifierName1}() {{\n_;\n}}");
            yield return GenerateRow(string.Empty, modifierName2, null, $"modifier {modifierName2}() {{\n_;\n}}");
            yield return GenerateRow(oneElementParametersListCode, modifierName1, null, $"modifier {modifierName1}({oneElementParametersListCode}) {{\n_;\n}}");
            yield return GenerateRow(twoElementParametersListCode, modifierName2, null, $"modifier {modifierName2}({twoElementParametersListCode}) {{\n_;\n}}");
            yield return GenerateRow(threeElementParametersListCode, modifierName1, null, $"modifier {modifierName1}({threeElementParametersListCode}) {{\n_;\n}}");
            yield return GenerateRow(null, modifierName1, instructionsCode1, $"modifier {modifierName1}() {{\n{instructionsCode1}\n_;\n}}");
            yield return GenerateRow(string.Empty, modifierName2, instructionsCode2, $"modifier {modifierName2}() {{\n{instructionsCode2}\n_;\n}}");
            yield return GenerateRow(oneElementParametersListCode, modifierName1, instructionsCode1, $"modifier {modifierName1}({oneElementParametersListCode}) {{\n{instructionsCode1}\n_;\n}}");
            yield return GenerateRow(twoElementParametersListCode, modifierName2, instructionsCode2, $"modifier {modifierName2}({twoElementParametersListCode}) {{\n{instructionsCode2}\n_;\n}}");
            yield return GenerateRow(threeElementParametersListCode, modifierName1, instructionsCode1, $"modifier {modifierName1}({threeElementParametersListCode}) {{\n{instructionsCode1}\n_;\n}}");
        }

        static object[] GenerateRow(string parametersListCode, string name, string instructionsListCode, string expected)
        {
            var parametersListMock = parametersListCode != null ? mockHelper.PrepareMock(parametersListCode, true) : null;
            var instructionsListMock = instructionsListCode != null ? instructionsListMockHelper.PrepareMock(instructionsListCode, false, !string.IsNullOrWhiteSpace(instructionsListCode)) : null;

            var m = new Modifier()
            {
                Name = name,
                Parameters = parametersListMock,
                Instructions = instructionsListMock
            };
            return new object[] { m, expected };
        }
    }
}