using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGeneratorTests.Model.Helpers;
using System;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ContractEventTests
    {
        private static readonly ParametersListMockCreator mockHelper = new ParametersListMockCreator();

        [TestCleanup]
        public void Cleanup()
        {
            mockHelper.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyNameGenerateCodeTest()
        {
            new ContractEvent().GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyNameGenerateCallCodeTest()
        {
            new ContractEvent().GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidNameTest()
        {
            new ContractEvent()
            {
                Name = "123"
            };
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForGenerateCodeTest), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(ContractEvent e, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(e != null);
            Assert.AreEqual(expected, e.GenerateCode());
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForGenerateCallCodeTest), DynamicDataSourceType.Method)]
        public void GenerateCallCodeTest(ContractEvent e, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(e != null);
            Assert.AreEqual(expected, e.GenerateCallCode());
        }

        static IEnumerable<object[]> GetDataForGenerateCodeTest()
        {
            string name1 = "Name";
            string name2 = "Name2";
            var oneElementParametersListCode = "Type1 Name1";
            var twoElementParametersListCode = "Type1 Name1, Type2 Name2";
            var threeElementParametersListCode = "Type1 Name1, Type2 Name2, Type3 Name3";

            yield return GenerateRow(null, name1, $"event {name1}()");
            yield return GenerateRow(null, name2, $"event {name2}()");
            yield return GenerateRow(string.Empty, name1, $"event {name1}()");
            yield return GenerateRow(string.Empty, name2, $"event {name2}()");
            yield return GenerateRow(oneElementParametersListCode, name1, $"event {name1}({oneElementParametersListCode})");
            yield return GenerateRow(twoElementParametersListCode, name2, $"event {name2}({twoElementParametersListCode})");
            yield return GenerateRow(threeElementParametersListCode, name2, $"event {name2}({threeElementParametersListCode})");
        }

        static IEnumerable<object[]> GetDataForGenerateCallCodeTest()
        {
            string name1 = "Name";
            string name2 = "Name2";
            yield return new object[] { new ContractEvent() { Name = name1 }, name1 };
            yield return new object[] { new ContractEvent() { Name = name2 }, name2 };
        }

        static object[] GenerateRow(string parametersListCode, string name, string expected)
        {
            var parametersListMock = parametersListCode != null ? mockHelper.PrepareMock(parametersListCode, true) : null;

            var e = new ContractEvent()
            {
                Name = name,
                Parameters = parametersListMock
            };
            return new object[] { e, expected };
        }
    }
}