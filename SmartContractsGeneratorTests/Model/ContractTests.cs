using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGeneratorTests.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ContractTests
    {
        private static readonly ConstructorMockCreator constructorMock = new ConstructorMockCreator();
        private static readonly FunctionMockCreator functionMock = new FunctionMockCreator();
        private static readonly EventMockCreator eventMock = new EventMockCreator();
        private static readonly ContractPropertyMockCreator propertyMock = new ContractPropertyMockCreator();

        [TestCleanup]
        public void Cleanup()
        {
            constructorMock.Dispose();
            functionMock.Dispose();
            eventMock.Dispose();
            propertyMock.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyContractNameTest()
        {
            new Contract().GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidContractNameTest()
        {
            new Contract()
            {
                Name = "123"
            };
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void ContractTest(Contract c, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(c != null);
            Assert.AreEqual(expected, c.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            string name1 = "name1";
            string name2 = "name2";
            string constructorCode1 = "CONSTRUCTOR CODE 1";
            string constructorCode2 = "CONSTRUCTOR CODE 2";
            string constructorCode3 = "CONSTRUCTOR CODE 3";
            string eventCode1 = "EVENT CODE 1";
            string eventCode2 = "EVENT CODE 2";
            string eventCode3 = "EVENT CODE 3";
            string funcCode1 = "FUNC CODE 1";
            string funcCode2 = "FUNC CODE 2";
            string funcCode3 = "FUNC CODE 3";
            string propertyCode1 = "PROPERTY CODE 1";
            string propertyCode2 = "PROPERTY CODE 2";
            string propertyCode3 = "PROPERTY CODE 3";

            yield return GenerateRow(null, name1, null, null, null, $"contract {name1} {{\n}}");
            yield return GenerateRow(constructorCode1, name1, null, null, null, $"contract {name1} {{\n{constructorCode1}\n}}");
            yield return GenerateRow(constructorCode2, name2, null, null, null, $"contract {name2} {{\n{constructorCode2}\n}}");
            yield return GenerateRow(constructorCode3, name1, null, null, null, $"contract {name1} {{\n{constructorCode3}\n}}");
            yield return GenerateRow(null, name1, new List<string>() { eventCode1 }, null, null, $"contract {name1} {{\n{eventCode1};\n}}");
            yield return GenerateRow(null, name1, new List<string>() { eventCode1, eventCode2 }, null, null, $"contract {name1} {{\n{eventCode1};\n{eventCode2};\n}}");
            yield return GenerateRow(null, name1, new List<string>() { eventCode1, eventCode2, eventCode3 }, null, null, $"contract {name1} {{\n{eventCode1};\n{eventCode2};\n{eventCode3};\n}}");
            yield return GenerateRow(null, name1, null, new List<string>() { funcCode1 }, null, $"contract {name1} {{\n{funcCode1}\n}}");
            yield return GenerateRow(null, name1, null, new List<string>() { funcCode1, funcCode2 }, null, $"contract {name1} {{\n{funcCode1}\n\n{funcCode2}\n}}");
            yield return GenerateRow(null, name1, null, new List<string>() { funcCode1, funcCode2, funcCode3 }, null, $"contract {name1} {{\n{funcCode1}\n\n{funcCode2}\n\n{funcCode3}\n}}");
            yield return GenerateRow(constructorCode1, name1, null, new List<string>() { funcCode1, funcCode2, funcCode3 }, null, $"contract {name1} {{\n{constructorCode1}\n\n{funcCode1}\n\n{funcCode2}\n\n{funcCode3}\n}}");
            yield return GenerateRow(constructorCode1, name1, new List<string>() { eventCode1, eventCode2, eventCode3 }, null, null, $"contract {name1} {{\n{constructorCode1}\n\n{eventCode1};\n{eventCode2};\n{eventCode3};\n}}");
            yield return GenerateRow(constructorCode1, name1, new List<string>() { eventCode1, eventCode2, eventCode3 }, new List<string>() { funcCode1, funcCode2, funcCode3 }, null, $"contract {name1} {{\n{constructorCode1}\n\n{eventCode1};\n{eventCode2};\n{eventCode3};\n\n{funcCode1}\n\n{funcCode2}\n\n{funcCode3}\n}}");
            yield return GenerateRow(null, name1, null, null, new List<string>() { eventCode1 }, $"contract {name1} {{\n{eventCode1};\n}}");
            yield return GenerateRow(null, name1, null, null, new List<string>() { propertyCode1 }, $"contract {name1} {{\n{propertyCode1};\n}}");
            yield return GenerateRow(null, name1, null, null, new List<string>() { propertyCode1, propertyCode2 }, $"contract {name1} {{\n{propertyCode1};\n{propertyCode2};\n}}");
            yield return GenerateRow(null, name1, null, null, new List<string>() { propertyCode1, propertyCode2, propertyCode3 }, $"contract {name1} {{\n{propertyCode1};\n{propertyCode2};\n{propertyCode3};\n}}");
            yield return GenerateRow(constructorCode1, name1, new List<string>() { eventCode1, eventCode2, eventCode3 }, new List<string>() { funcCode1, funcCode2, funcCode3 }, new List<string>() { propertyCode1, propertyCode2, propertyCode3 }, $"contract {name1} {{\n{constructorCode1}\n\n{propertyCode1};\n{propertyCode2};\n{propertyCode3};\n\n{eventCode1};\n{eventCode2};\n{eventCode3};\n\n{funcCode1}\n\n{funcCode2}\n\n{funcCode3}\n}}");
        }

        static object[] GenerateRow(string constructorCode, string name, List<string> expectedEventsCode, List<string> expectedFunctionsCode, List<string> expectedPropertyCode, string expected)
        {
            var c = new Contract()
            {
                Name = name
            };

            if (constructorCode != null)
            {
                c.Constructor = constructorMock.PrepareMock(constructorCode);
            }

            if (expectedPropertyCode != null)
            {
                c.Properties = expectedPropertyCode.Select(code => propertyMock.PrepareMock(null, code)).ToList();
            }

            if (expectedEventsCode != null)
            {
                c.Events = expectedEventsCode.Select(code => eventMock.PrepareMock(code, null)).ToList();
            }

            if (expectedFunctionsCode != null)
            {
                c.Functions = expectedFunctionsCode.Select(code => functionMock.PrepareMock(code, null)).ToList();
            }

            return new object[] { c, expected };
        }
    }
}