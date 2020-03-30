using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGeneratorTests.Model.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SmartContractsGenerator.Model.AbstractPatterns.Tests
{
    [TestClass()]
    public class ParametersListTests
    {
        private static readonly VariableMocksCreator mockHelper = new VariableMocksCreator();

        [TestCleanup]
        public void Cleanup()
        {
            mockHelper.Dispose();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(ParametersList parametersList, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(parametersList != null);
            Assert.AreEqual(expected, parametersList.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            var parameters = new List<Variable>();

            var expectedAnswers = new Dictionary<int, string>()
            {
                { 0, string.Empty }
            };

            int bound = 4;
            for (int i = 1; i < bound; i++)
            {
                var name = $"Type{i} Name{i}";
                var p = mockHelper.PrepareMock(string.Empty, name);
                parameters.Add(p);

                for (int j = i; j < bound; j++)
                {
                    if (expectedAnswers.ContainsKey(j))
                    {
                        expectedAnswers[j] += $", {name}";
                    }
                    else
                    {
                        expectedAnswers.Add(j, name);
                    }
                }
            }

            List<object[]> data = new List<object[]>()
            {
                new object[] { new ParametersList(), string.Empty }
            };

            foreach (var entry in expectedAnswers)
            {
                var pl = new ParametersList()
                {
                    Parameters = parameters.Take(entry.Key).ToList()
                };
                data.Add(new object[] { pl, entry.Value });
            }

            return data;
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForGenerateCallCodeTest), DynamicDataSourceType.Method)]
        public void GenerateCallCodeTest(ParametersList parametersList, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(parametersList != null);
            Assert.AreEqual(expected, parametersList.GenerateCallCode());
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
            
            yield return new object[] { new ParametersList(), string.Empty };

            var paramsList = new List<Variable>() { p1 };
            yield return new object[] { new ParametersList() { Parameters = paramsList }, name1 };

            var p2 = new Variable()
            {
                Name = name2,
                Type = type2
            };
            paramsList.Add(p2);
            yield return new object[] { new ParametersList() { Parameters = paramsList }, $"{name1}, {name2}" };

            var p3 = new Variable()
            {
                Name = name3,
                Type = type3
            };
            paramsList.Add(p3);
            yield return new object[] { new ParametersList() { Parameters = paramsList }, $"{name1}, {name2}, {name3}" };
        }
    }
}