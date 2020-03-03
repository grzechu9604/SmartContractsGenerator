using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ParameterTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(Parameter p, string expected, string message)
        {
            Assert.AreEqual(expected, p.GenerateCode(), message);
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            var name = "_testName";
            var type = "testType";

            var p = new Parameter()
            {
                Name = name,
                Type = type
            };

            return new[]
            {
                new object[] {p, $"{type} {name}", "Simple parameter test failed" }
            };
        }
    }
}