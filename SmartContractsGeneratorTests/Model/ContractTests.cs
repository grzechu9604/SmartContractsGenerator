using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using System;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ContractTests
    {
        private static readonly List<AutoMock> mocks = new List<AutoMock>();

        [TestCleanup]
        public void Cleanup()
        {
            mocks.ForEach(m => m.Dispose());
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyConstructorNameTest()
        {
            new Contract().GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidConstructorNameTest()
        {
            new Contract()
            {
                Name = "123"
            };
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void ConstructorTest(Contract c, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(c != null);
            Assert.AreEqual(expected, c.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            List<object[]> data = new List<object[]>();

            string name1 = "name1";
            string name2 = "name2";
            string constructorCode1 = "CONSTRUCTOR CODE 1";
            string constructorCode2 = "CONSTRUCTOR CODE 2";
            string constructorCode3 = "CONSTRUCTOR CODE 3";

            data.Add(GenerateRow(null, name1, $"contract {name1} {{\n}}"));
            data.Add(GenerateRow(constructorCode1, name1, $"contract {name1} {{\n{constructorCode1}\n}}"));
            data.Add(GenerateRow(constructorCode2, name2, $"contract {name2} {{\n{constructorCode2}\n}}"));
            data.Add(GenerateRow(constructorCode3, name1, $"contract {name1} {{\n{constructorCode3}\n}}"));
           
            return data;
        }

        static object[] GenerateRow(string constructorCode, string name, string expected)
        {
            var c = new Contract()
            {
                Name = name
            };

            if (constructorCode != null)
            {
                var autoMock = AutoMock.GetLoose();
                mocks.Add(autoMock);

                autoMock.Mock<Constructor>()
                    .Setup(x => x.GenerateCode())
                    .Returns(constructorCode);

                c.Constructor = autoMock.Create<Constructor>();
            }

            return new object[] { c, expected };
        }
    }
}