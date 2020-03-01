using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Model.AbstractPatterns;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.AbstractPatterns.Tests
{
    [TestClass()]
    public class ParametersListTests
    {
        [TestMethod()]
        public void GenerateCodeNullListTest()
        {
            var pl = new ParametersList();
            Assert.AreEqual(string.Empty, pl.GenerateCode());
        }

        [TestMethod()]
        public void GenerateCodeEmptyListTest()
        {
            var pl = new ParametersList()
            {
                Parameters = new List<Parameter>()
            };
            Assert.AreEqual(string.Empty, pl.GenerateCode());
        }

        [TestMethod()]
        public void GenerateCodeOneElementListTest()
        {
            var name = "Name";
            var type = "Type";
            var p = new Parameter()
            {
                Name = name,
                Type = type
            };

            var pl = new ParametersList()
            {
                Parameters = new List<Parameter>() { p }
            };
            Assert.AreEqual($"{type} {name}", pl.GenerateCode());
        }

        [TestMethod()]
        public void GenerateCodeMultiElementListTest()
        {
            var name = "Name";
            var type = "Type";
            var p = new Parameter()
            {
                Name = name,
                Type = type
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

            var pl = new ParametersList()
            {
                Parameters = new List<Parameter>() { p, p2, p3 }
            };
            Assert.AreEqual($"{type} {name}, {type2} {name2}, {type3} {name3}", pl.GenerateCode());
        }
    }
}