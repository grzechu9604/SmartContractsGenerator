using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace SmartContractsGenerator.Model.AbstractPatterns.Tests
{
    [TestClass()]
    public class ParametersListTests
    {
        private static List<AutoMock> _mocks = new List<AutoMock>();

        [TestCleanup]
        public void Cleanup()
        {
            _mocks.ForEach(m => m.Dispose());
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
                var p = PrepareParameterMock(name);
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

        static Parameter PrepareParameterMock(string expected)
        {
            var mock = AutoMock.GetLoose();
            _mocks.Add(mock);

            mock.Mock<Parameter>()
                .Setup(x => x.GenerateCode())
                .Returns(expected);

            var preparedMock = mock.Create<Parameter>();

            return preparedMock;
        }
    }
}