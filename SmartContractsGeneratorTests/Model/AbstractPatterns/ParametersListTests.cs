using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Model.AbstractPatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartContractsGenerator.Model.AbstractPatterns.Tests
{
    [TestClass()]
    public class ParametersListTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeNullListTest(ParametersList parametersList, string expected)
        {
            Assert.AreEqual(expected, parametersList.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            var name1 = "Name";
            var type1 = "Type";
            var p1 = new Parameter()
            {
                Name = name1,
                Type = type1
            };

            var name2 = "Name2";
            var type2 = "Type2";
            var p2 = new Parameter()
            {
                Name = name2,
                Type = type2
            };

            var name3 = "Name3";
            var type3 = "Type3";
            var p3 = new Parameter()
            {
                Name = name3,
                Type = type3
            };

            var parameters = new List<Parameter>() { p1, p2, p3 };
            var expectedAnswers = new Dictionary<int, string>()
            {
                { 0, string.Empty },
                { 1, $"{type1} {name1}" },
                { 2, $"{type1} {name1}, {type2} {name2}" },
                { 3, $"{type1} {name1}, {type2} {name2}, {type3} {name3}" }
            };

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