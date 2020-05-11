using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGeneratorTests.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.AbstractPatterns.Tests
{
    [TestClass()]
    public class CallingParametersListTests
    {
        private static readonly VariableMocksCreator mockHelper = new VariableMocksCreator();

        [TestCleanup]
        public void Cleanup()
        {
            mockHelper.Dispose();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForGenerateCallCodeTest), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(CallingParametersList parametersList, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(parametersList != null);
            Assert.AreEqual(expected, parametersList.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForGenerateCallCodeTest()
        {
            var name1 = "Name1";
            var name2 = "Name2";
            var name3 = "Name3";

            var type1 = "type1";
            var type2 = "type2";
            var type3 = "type3";

            var p1 = new Variable()
            {
                Name = name1,
                Type = type1
            };

            yield return new object[] { new CallingParametersList(), string.Empty };

            var paramsList = new List<IAssignable>() { p1 };
            yield return new object[] { new CallingParametersList() { Parameters = paramsList }, name1 };

            var p2 = new Variable()
            {
                Name = name2,
                Type = type2
            };
            paramsList.Add(p2);
            yield return new object[] { new CallingParametersList() { Parameters = paramsList }, $"{name1}, {name2}" };

            var p3 = new Variable()
            {
                Name = name3,
                Type = type3
            };
            paramsList.Add(p3);
            yield return new object[] { new CallingParametersList() { Parameters = paramsList }, $"{name1}, {name2}, {name3}" };
        }
    }
}