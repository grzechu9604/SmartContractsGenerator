using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model.AbstractPatterns;
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
        private static readonly ModifierMockCreator modifierMock = new ModifierMockCreator();

        [TestCleanup]
        public void Cleanup()
        {
            constructorMock.Dispose();
            functionMock.Dispose();
            eventMock.Dispose();
            propertyMock.Dispose();
            modifierMock.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyContractNameTest()
        {
            new Contract().GenerateCode(new Indentation());
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
        public void ContractTest(Contract c, Indentation indentation, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(c != null);
            Assert.AreEqual(expected, c.GenerateCode(indentation));
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
            string modifierCode1 = "MODIFIER CODE 1";
            string modifierCode2 = "MODIFIER CODE 2";
            string modifierCode3 = "MODIFIER CODE 3";

            yield return GenerateRow(null, name1, null, null, null, null, $"contract {name1} {{\n}}");
            yield return GenerateRow(constructorCode1, name1, null, null, null, null, $"contract {name1} {{\n\t{constructorCode1}\n}}");
            yield return GenerateRow(constructorCode2, name2, null, null, null, null, $"contract {name2} {{\n\t{constructorCode2}\n}}");
            yield return GenerateRow(constructorCode3, name1, null, null, null, null, $"contract {name1} {{\n\t{constructorCode3}\n}}");
            yield return GenerateRow(null, name1, new List<string>() { eventCode1 }, null, null, null, $"contract {name1} {{\n\t{eventCode1};\n}}");
            yield return GenerateRow(null, name1, new List<string>() { eventCode1, eventCode2 }, null, null, null, $"contract {name1} {{\n\t{eventCode1};\n\t{eventCode2};\n}}");
            yield return GenerateRow(null, name1, new List<string>() { eventCode1, eventCode2, eventCode3 }, null, null, null, $"contract {name1} {{\n\t{eventCode1};\n\t{eventCode2};\n\t{eventCode3};\n}}");
            yield return GenerateRow(null, name1, null, new List<string>() { funcCode1 }, null, null, $"contract {name1} {{\n\t{funcCode1}\n}}");
            yield return GenerateRow(null, name1, null, new List<string>() { funcCode1, funcCode2 }, null, null, $"contract {name1} {{\n\t{funcCode1}\n\n\t{funcCode2}\n}}");
            yield return GenerateRow(null, name1, null, new List<string>() { funcCode1, funcCode2, funcCode3 }, null, null, $"contract {name1} {{\n\t{funcCode1}\n\n\t{funcCode2}\n\n\t{funcCode3}\n}}");
            yield return GenerateRow(constructorCode1, name1, null, new List<string>() { funcCode1, funcCode2, funcCode3 }, null, null, $"contract {name1} {{\n\t{constructorCode1}\n\n\t{funcCode1}\n\n\t{funcCode2}\n\n\t{funcCode3}\n}}");
            yield return GenerateRow(constructorCode1, name1, new List<string>() { eventCode1, eventCode2, eventCode3 }, null, null, null, $"contract {name1} {{\n\t{eventCode1};\n\t{eventCode2};\n\t{eventCode3};\n\n\t{constructorCode1}\n}}");
            yield return GenerateRow(constructorCode1, name1, new List<string>() { eventCode1, eventCode2, eventCode3 }, new List<string>() { funcCode1, funcCode2, funcCode3 }, null, null, $"contract {name1} {{\n\t{eventCode1};\n\t{eventCode2};\n\t{eventCode3};\n\n\t{constructorCode1}\n\n\t{funcCode1}\n\n\t{funcCode2}\n\n\t{funcCode3}\n}}");
            yield return GenerateRow(null, name1, null, null, new List<string>() { eventCode1 }, null, $"contract {name1} {{\n\t{eventCode1};\n}}");
            yield return GenerateRow(null, name1, null, null, new List<string>() { propertyCode1 }, null, $"contract {name1} {{\n\t{propertyCode1};\n}}");
            yield return GenerateRow(null, name1, null, null, new List<string>() { propertyCode1, propertyCode2 }, null, $"contract {name1} {{\n\t{propertyCode1};\n\t{propertyCode2};\n}}");
            yield return GenerateRow(null, name1, null, null, new List<string>() { propertyCode1, propertyCode2, propertyCode3 }, null, $"contract {name1} {{\n\t{propertyCode1};\n\t{propertyCode2};\n\t{propertyCode3};\n}}");
            yield return GenerateRow(constructorCode1, name1, new List<string>() { eventCode1, eventCode2, eventCode3 }, new List<string>() { funcCode1, funcCode2, funcCode3 }, new List<string>() { propertyCode1, propertyCode2, propertyCode3 }, null, $"contract {name1} {{\n\t{propertyCode1};\n\t{propertyCode2};\n\t{propertyCode3};\n\n\t{eventCode1};\n\t{eventCode2};\n\t{eventCode3};\n\n\t{constructorCode1}\n\n\t{funcCode1}\n\n\t{funcCode2}\n\n\t{funcCode3}\n}}");
            yield return GenerateRow(null, name1, null, null, null, new List<string>() { modifierCode1 }, $"contract {name1} {{\n\t{modifierCode1}\n}}");
            yield return GenerateRow(null, name1, null, null, null, new List<string>() { modifierCode1, modifierCode2 }, $"contract {name1} {{\n\t{modifierCode1}\n\n\t{modifierCode2}\n}}");
            yield return GenerateRow(null, name1, null, null, null, new List<string>() { modifierCode1, modifierCode2, modifierCode3 }, $"contract {name1} {{\n\t{modifierCode1}\n\n\t{modifierCode2}\n\n\t{modifierCode3}\n}}");
            yield return GenerateRow(constructorCode1, name1, new List<string>() { eventCode1, eventCode2, eventCode3 }, new List<string>() { funcCode1, funcCode2, funcCode3 }, new List<string>() { propertyCode1, propertyCode2, propertyCode3 }, new List<string>() { modifierCode1, modifierCode2, modifierCode3 }, $"contract {name1} {{\n\t{propertyCode1};\n\t{propertyCode2};\n\t{propertyCode3};\n\n\t{eventCode1};\n\t{eventCode2};\n\t{eventCode3};\n\n\t{constructorCode1}\n\n\t{modifierCode1}\n\n\t{modifierCode2}\n\n\t{modifierCode3}\n\n\t{funcCode1}\n\n\t{funcCode2}\n\n\t{funcCode3}\n}}");
        }

        static object[] GenerateRow(string constructorCode, string name, List<string> expectedEventsCode, List<string> expectedFunctionsCode, List<string> expectedPropertyCode, List<string> expectedModifierCode, string expected)
        {
            var indentation = new Indentation();
            var indentationLevelOne = indentation.GetIndentationWithIncrementedLevel();
            var c = new Contract()
            {
                Name = name
            };

            if (constructorCode != null)
            {
                c.Constructor = constructorMock.PrepareMock(constructorCode, indentationLevelOne);
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
                c.Functions = expectedFunctionsCode.Select(code => functionMock.PrepareMock(code, null, indentationLevelOne)).ToList();
            }

            if (expectedModifierCode != null)
            {
                c.Modifiers = expectedModifierCode.Select(code => modifierMock.PrepareMock(code, indentationLevelOne)).ToList();
            }

            return new object[] { c, indentation, expected };
        }
    }
}