using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGeneratorTests.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ModifierTests
    {
        private static readonly ParametersListMockCreator mockHelper = new ParametersListMockCreator();

        [TestCleanup]
        public void Cleanup()
        {
            mockHelper.Dispose();
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

            yield return GenerateRow(null, modifierName1, $"modifier {modifierName1}() {{\n_;\n}}");
            yield return GenerateRow(string.Empty, modifierName2, $"modifier {modifierName2}() {{\n_;\n}}");
            yield return GenerateRow(oneElementParametersListCode, modifierName1, $"modifier {modifierName1}({oneElementParametersListCode}) {{\n_;\n}}");
            yield return GenerateRow(twoElementParametersListCode, modifierName2, $"modifier {modifierName2}({twoElementParametersListCode}) {{\n_;\n}}");
            yield return GenerateRow(threeElementParametersListCode, modifierName1, $"modifier {modifierName1}({threeElementParametersListCode}) {{\n_;\n}}");
        }

        static object[] GenerateRow(string parametersListCode, string name, string expected)
        {
            var parametersListMock = parametersListCode != null ? mockHelper.PrepareMock(parametersListCode) : null;
            var m = new Modifier()
            {
                Name = name,
                Parameters = parametersListMock
            };
            return new object[] { m, expected };
        }
    }
}