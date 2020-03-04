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
    public class ModifierTests
    {
        private static readonly ParametersListMockCreator mockHelper = new ParametersListMockCreator();

        [TestCleanup]
        public void Cleanup()
        {
            mockHelper.Dispose();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void ConstructorTest(Modifier m, string expected)
        {
            Assert.AreEqual(expected, m.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            List<object[]> data = new List<object[]>();

            string modifierName1 = "Name";
            string modifierName2 = "Name2";
            var oneElementParametersListCode = "Type1 Name1";
            var twoElementParametersListCode = "Type1 Name1, Type2 Name2";
            var threeElementParametersListCode = "Type1 Name1, Type2 Name2, Type3 Name3";

            data.Add(GenerateRow(null, modifierName1, $"modifier {modifierName1}() {{\n_;\n}}"));
            data.Add(GenerateRow(string.Empty, modifierName2, $"modifier {modifierName2}() {{\n_;\n}}"));
            data.Add(GenerateRow(oneElementParametersListCode, modifierName1, $"modifier {modifierName1}({oneElementParametersListCode}) {{\n_;\n}}"));
            data.Add(GenerateRow(twoElementParametersListCode, modifierName2, $"modifier {modifierName2}({twoElementParametersListCode}) {{\n_;\n}}"));
            data.Add(GenerateRow(threeElementParametersListCode, modifierName1, $"modifier {modifierName1}({threeElementParametersListCode}) {{\n_;\n}}"));

            return data;
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