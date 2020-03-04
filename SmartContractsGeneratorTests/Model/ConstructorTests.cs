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
    public class ConstructorTests
    {
        private static readonly ParametersListMockCreator mockHelper = new ParametersListMockCreator();

        [TestCleanup]
        public void Cleanup()
        {
            mockHelper.Dispose();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void ConstructorTest(Constructor c, string expected)
        {
            Assert.AreEqual(expected, c.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            List<object[]> data = new List<object[]>();

            string publicModifier = "public";
            string privateModifier = "private";
            var oneElementParametersListCode = "Type1 Name1";
            var twoElementParametersListCode = "Type1 Name1, Type2 Name2";
            var threeElementParametersListCode = "Type1 Name1, Type2 Name2, Type3 Name3";


            data.Add(GenerateRow(null, publicModifier, $"constructor() {publicModifier} {{\n}}"));
            data.Add(GenerateRow(string.Empty, privateModifier, $"constructor() {privateModifier} {{\n}}"));
            data.Add(GenerateRow(oneElementParametersListCode, publicModifier, $"constructor({oneElementParametersListCode}) {publicModifier} {{\n}}"));
            data.Add(GenerateRow(twoElementParametersListCode, publicModifier, $"constructor({twoElementParametersListCode}) {publicModifier} {{\n}}"));
            data.Add(GenerateRow(threeElementParametersListCode, publicModifier, $"constructor({threeElementParametersListCode}) {publicModifier} {{\n}}"));

            return data;
        }

        static object[] GenerateRow(string parametersListCode, string modifier, string expected)
        {
            var parametersListMock = parametersListCode != null ? mockHelper.PrepareMock(parametersListCode) : null;
            var c = new Constructor()
            {
                Modifier = modifier,
                Parameters = parametersListMock
            };
            return new object[] { c, expected };
        }
    }
}