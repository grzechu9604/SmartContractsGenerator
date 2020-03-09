using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model.Enums;
using SmartContractsGeneratorTests.Model.Helpers;
using System.Collections.Generic;

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

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void MissingVisibilityTest()
        {
            new Constructor().GenerateCode();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidVisibilitySpecifierException))]
        [DynamicData(nameof(GetInvalidVisibilities), DynamicDataSourceType.Method)]
        public void InvalidVisibilitySpecifierTest(Visibility v)
        {
            new Constructor()
            {
                Visibility = v
            };
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

            var oneElementParametersListCode = "Type1 Name1";
            var twoElementParametersListCode = "Type1 Name1, Type2 Name2";
            var threeElementParametersListCode = "Type1 Name1, Type2 Name2, Type3 Name3";

            data.Add(GenerateRow(null, Visibility.Public, $"constructor() public {{\n}}"));
            data.Add(GenerateRow(string.Empty, Visibility.Internal, $"constructor() internal {{\n}}"));
            data.Add(GenerateRow(oneElementParametersListCode, Visibility.Public, $"constructor({oneElementParametersListCode}) public {{\n}}"));
            data.Add(GenerateRow(twoElementParametersListCode, Visibility.Public, $"constructor({twoElementParametersListCode}) public {{\n}}"));
            data.Add(GenerateRow(threeElementParametersListCode, Visibility.Public, $"constructor({threeElementParametersListCode}) public {{\n}}"));

            return data;
        }

        static object[] GenerateRow(string parametersListCode, Visibility visibility, string expected)
        {
            var parametersListMock = parametersListCode != null ? mockHelper.PrepareMock(parametersListCode) : null;
            var c = new Constructor()
            {
                Visibility = visibility,
                Parameters = parametersListMock
            };
            return new object[] { c, expected };
        }

        static IEnumerable<object[]> GetInvalidVisibilities()
        {
            return new List<object[]> ()
            {
                new object[] { Visibility.External },
                new object[] { Visibility.Private }
            };
        }
    }
}