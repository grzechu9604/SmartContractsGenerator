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
            Assert.AreEqual(expected, parametersList.GenerateCode(true));
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
                var p = mockHelper.PrepareMock(string.Empty, name, true);
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
    }
}