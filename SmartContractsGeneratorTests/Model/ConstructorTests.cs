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
            System.Diagnostics.Contracts.Contract.Requires(c != null);
            Assert.AreEqual(expected, c.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            List<object[]> data = new List<object[]>();

            string publicVisibility = "public";
            string privateVisibility = "private";
            var oneElementParametersListCode = "Type1 Name1";
            var twoElementParametersListCode = "Type1 Name1, Type2 Name2";
            var threeElementParametersListCode = "Type1 Name1, Type2 Name2, Type3 Name3";


            data.Add(GenerateRow(null, publicVisibility, $"constructor() {publicVisibility} {{\n}}"));
            data.Add(GenerateRow(string.Empty, privateVisibility, $"constructor() {privateVisibility} {{\n}}"));
            data.Add(GenerateRow(oneElementParametersListCode, publicVisibility, $"constructor({oneElementParametersListCode}) {publicVisibility} {{\n}}"));
            data.Add(GenerateRow(twoElementParametersListCode, publicVisibility, $"constructor({twoElementParametersListCode}) {publicVisibility} {{\n}}"));
            data.Add(GenerateRow(threeElementParametersListCode, publicVisibility, $"constructor({threeElementParametersListCode}) {publicVisibility} {{\n}}"));

            return data;
        }

        static object[] GenerateRow(string parametersListCode, string visibility, string expected)
        {
            var parametersListMock = parametersListCode != null ? mockHelper.PrepareMock(parametersListCode) : null;
            var c = new Constructor()
            {
                Visibility = visibility,
                Parameters = parametersListMock
            };
            return new object[] { c, expected };
        }
    }
}