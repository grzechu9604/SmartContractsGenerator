using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ContractTests
    {
        [TestMethod()]
        public void EmptyContractTest()
        {
            string contractName = "Test";
            Contract c = new Contract()
            {
                Name = contractName
            };

            string correctContract = $"contract {contractName} {{\n}}";
            Assert.AreEqual(correctContract, c.GenerateCode());
        }

        [TestMethod()]
        public void EmptyConstructorContractTest()
        {
            var modifier = "public";

            var ctor = new Constructor()
            {
                Modifier = modifier
            };

            string contractName = "Test";
            Contract c = new Contract()
            {
                Name = contractName,
                Constructor = ctor
            };

            string correctContract = $"contract {contractName} {{\nconstructor() {modifier} {{\n}}\n}}";
            Assert.AreEqual(correctContract, c.GenerateCode());
        }

        [TestMethod()]
        public void EmptyConstructorWithParametersContractTest()
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

            string contractName = "Test";
            Contract contract = new Contract()
            {
                Name = contractName,
                Constructor = c
            };

            string correctContract = $"contract {contractName} {{\nconstructor({type1} {name1}, {type2} {name2}, {type3} {name3}) {cModifier} {{\n}}\n}}";
            Assert.AreEqual(correctContract, contract.GenerateCode());
        }

        [TestMethod()]
        public void EmptyConstructorWithOneParameterContractTest()
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

            string contractName = "Test";
            Contract contract = new Contract()
            {
                Name = contractName,
                Constructor = c
            };

            Assert.AreEqual($"contract {contractName} {{\nconstructor({type1} {name1}) {cModifier} {{\n}}\n}}", contract.GenerateCode());
        }
    }
}