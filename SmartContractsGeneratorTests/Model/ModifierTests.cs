using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.AbstractPatterns;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ModifierTests
    {
        [TestMethod()]
        public void EmptyModifierTest()
        {
            var name = "Name";
            var m = new Modifier()
            {
                Name = name
            };

            Assert.AreEqual($"modifier {name}() {{\n_;\n}}", m.GenerateCode());
        }

        [TestMethod()]

        public void EmptyModifierWithOneParameterTest()
        {
            var name1 = "Name1";
            var type1 = "Type1";
            var p1 = new Parameter()
            {
                Name = name1,
                Type = type1
            };

            var pl = new ParametersList()
            {
                Parameters = new List<Parameter>() { p1 }
            };

            var name = "Name";
            var m = new Modifier()
            {
                Name = name,
                Parameters = pl
            };

            Assert.AreEqual($"modifier {name}({type1} {name1}) {{\n_;\n}}", m.GenerateCode());
        }

        [TestMethod()]

        public void EmptyModifierWithParametersTest()
        {
            var name1 = "Name1";
            var type1 = "Type1";
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

            var pl = new ParametersList()
            {
                Parameters = new List<Parameter>() { p1, p2, p3 }
            };

            var name = "Name";
            var m = new Modifier()
            {
                Name = name,
                Parameters = pl
            };

            Assert.AreEqual($"modifier {name}({type1} {name1}, {type2} {name2}, {type3} {name3}) {{\n_;\n}}", m.GenerateCode());
        }
    }
}