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
        private static readonly InstructionsListMockCreator instructionsListMockHelper = new InstructionsListMockCreator();

        [TestCleanup]
        public void Cleanup()
        {
            mockHelper.Dispose();
            instructionsListMockHelper.Dispose();
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
            var oneElementParametersListCode = "Type1 Name1";
            var twoElementParametersListCode = "Type1 Name1, Type2 Name2";
            var threeElementParametersListCode = "Type1 Name1, Type2 Name2, Type3 Name3";
            var instructionsCode1 = "INSTRUCTIONS CODE 1";
            var instructionsCode2 = "INSTRUCTIONS CODE 2";

            yield return GenerateRow(null, Visibility.Public, null, $"constructor() public {{\n}}");
            yield return GenerateRow(string.Empty, Visibility.Internal, null, $"constructor() internal {{\n}}");
            yield return GenerateRow(oneElementParametersListCode, Visibility.Public, null, $"constructor({oneElementParametersListCode}) public {{\n}}");
            yield return GenerateRow(twoElementParametersListCode, Visibility.Public, null, $"constructor({twoElementParametersListCode}) public {{\n}}");
            yield return GenerateRow(threeElementParametersListCode, Visibility.Public, null, $"constructor({threeElementParametersListCode}) public {{\n}}");
            yield return GenerateRow(null, Visibility.Public, instructionsCode1, $"constructor() public {{\n{instructionsCode1}\n}}");
            yield return GenerateRow(string.Empty, Visibility.Internal, instructionsCode2, $"constructor() internal {{\n{instructionsCode2}\n}}");
            yield return GenerateRow(oneElementParametersListCode, Visibility.Public, instructionsCode1, $"constructor({oneElementParametersListCode}) public {{\n{instructionsCode1}\n}}");
            yield return GenerateRow(twoElementParametersListCode, Visibility.Public, instructionsCode1, $"constructor({twoElementParametersListCode}) public {{\n{instructionsCode1}\n}}");
            yield return GenerateRow(threeElementParametersListCode, Visibility.Public, instructionsCode2, $"constructor({threeElementParametersListCode}) public {{\n{instructionsCode2}\n}}");
        }

        static object[] GenerateRow(string parametersListCode, Visibility visibility, string instructionsListCode, string expected)
        {
            var parametersListMock = parametersListCode != null ? mockHelper.PrepareMock(parametersListCode, true) : null;
            var instructionsListMock = instructionsListCode != null ? instructionsListMockHelper.PrepareMock(instructionsListCode, false, !string.IsNullOrWhiteSpace(instructionsListCode)) : null;

            var c = new Constructor()
            {
                Visibility = visibility,
                Parameters = parametersListMock,
                Instructions = instructionsListMock
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