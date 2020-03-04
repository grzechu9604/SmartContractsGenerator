using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGeneratorTests.Model.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SmartContractsGenerator.Model.AbstractPatterns.Tests
{
    [TestClass()]
    public class ParametersListTests
    {
        private static readonly ParametersMocksCreator mockHelper = new ParametersMocksCreator();

        [TestCleanup]
        public void Cleanup()
        {
            mockHelper.DisposeMocks();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodetTest(ParametersList parametersList, string expected)
        {
            Assert.AreEqual(expected, parametersList.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            var parameters = new List<Parameter>();

            var expectedAnswers = new Dictionary<int, string>()
            {
                { 0, string.Empty }
            };

            int bound = 4;
            for (int i = 1; i < bound; i++)
            {
                var name = $"Type{i} Name{i}";
                var p = mockHelper.PrepareParameterMock(name);
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