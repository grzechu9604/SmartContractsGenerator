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
                Name = "Test"
            };

            string correctContract = $"contract a {contractName} {{\n}}";
            Assert.AreEqual(correctContract, c.GenerateCode());
        }
    }
}