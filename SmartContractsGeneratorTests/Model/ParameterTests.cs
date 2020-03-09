using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ParameterTests
    {

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyParameterNameTest()
        {
            new Parameter().GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidParameterNameTest()
        {
            new Parameter()
            {
                Name = "123"
            };
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(Parameter p, string expected, string message)
        {
            System.Diagnostics.Contracts.Contract.Requires(p != null);
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

            yield return new object[] { p, $"{type} {name}", "Simple parameter test failed" };
        }
    }
}