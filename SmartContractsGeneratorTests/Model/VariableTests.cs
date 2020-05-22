using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model.Enums;
using System;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class VariableTests
    {
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidNameTest()
        {
            new Variable()
            {
                Name = "123"
            };
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyNameGenerateUsageCodeTest()
        {
            new Variable().GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyNameGenerateDeclarationCodeTest()
        {
            new Variable().GenerateDeclarationCode(false);
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyTypeGenerateDeclarationCodeTest()
        {
            new Variable()
            {
                Name = "name"
            }.GenerateDeclarationCode(false);
        }

        [TestMethod()]
        public void GenerateUsageCodeTest()
        {
            var name = "name1";
            var type = SolidityType.Bool;
            var v = new Variable()
            {
                Type = type,
                Name = name
            };

            Assert.AreEqual($"{name}", v.GenerateCode());
        }

        [TestMethod()]
        public void GenerateDeclarationCodeTest()
        {
            var name = "name1";
            var type = SolidityType.Bool;
            var v = new Variable()
            {
                Type = type,
                Name = name
            };

            Assert.AreEqual($"{type.GenerateCode(false)} {name}", v.GenerateDeclarationCode(false));
        }

        [TestMethod()]
        public void GenerateDeclarationCodeWithMemoryPointingTest()
        {
            var name = "name1";
            var type = SolidityType.Bool;
            var v = new Variable()
            {
                Type = type,
                Name = name
            };

            Assert.AreEqual($"{type.GenerateCode(true)} {name}", v.GenerateDeclarationCode(true));
        }
    }
}