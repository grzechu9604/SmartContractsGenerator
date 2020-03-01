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
        [TestMethod()]
        public void GenerateCodeTest()
        {
            var name = "_testName";
            var type = "testType";

            var p = new Parameter()
            {
                Name = name,
                Type = type
            };

            Assert.AreEqual($"{type} {name}", p.GenerateCode());
        }
    }
}