using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ConstructorTests
    {
        [TestMethod()]
        public void EmptyConstructorTest()
        {
            var cModifier = "public";
            var c = new Constructor()
            {
                Modifier = cModifier
            };

            Assert.AreEqual($"constructor() {cModifier} {{\n}}", c.GenerateCode());
        }

        public void EmptyConstructorWithOneParameterTest()
        {
            var name1 = "Name1";
            var type1 = "Type1";
            var p1 = new Parameter()
            {
                Name = name1,
                Type = type1
            };

            var cModifier = "public";
            var c = new Constructor()
            {
                Modifier = cModifier,
                Parameters = new List<Parameter>() { p1 }
            };

            Assert.AreEqual($"constructor({type1} {name1}) {cModifier} {{\n}}", c.GenerateCode());
        }

        public void EmptyConstructorWithParametersTest()
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
            
            var cModifier = "public";
            var c = new Constructor()
            {
                Modifier = cModifier,
                Parameters = new List<Parameter>() { p1, p2, p3 }
            };

            Assert.AreEqual($"constructor({type1} {name1}, {type2} {name2}, {type3} {name3}) {cModifier} {{\n}}", c.GenerateCode());
        }
    }
}