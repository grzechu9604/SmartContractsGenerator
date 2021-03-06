﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Model.Enums;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class CallTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDataForGenerateCodeTest), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(AbstractCall c, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(c != null);
            Assert.AreEqual(expected, c.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForGenerateCodeTest()
        {
            string funcName = "functionName";
            var function = new ContractFunction()
            {
                Name = funcName
            };
            var functionCall = new FunctionCall()
            {
                FunctionToCall = function
            };
            yield return new object[] { functionCall, $"{funcName}()" };

            string eventName = "eventName";
            var contractEvent = new ContractEvent()
            {
                Name = eventName
            };
            var eventCall = new EventCall()
            {
                EventToCall = contractEvent
            };
            yield return new object[] { eventCall, $"emit {eventName}()" };

            string modifierName = "modifierName";
            var modifier = new Modifier()
            {
                Name = modifierName
            };
            var modifierAppliance = new ModifierAppliance()
            {
                ModifierToApply = modifier
            };
            yield return new object[] { modifierAppliance, $"{modifierName}()" };

            var variables = new List<IAssignable>();
            var parameters = new CallingParametersList()
            {
                Parameters = variables
            };

            functionCall.Parameters = parameters;
            yield return new object[] { functionCall, $"{funcName}()" };

            eventCall.Parameters = parameters;
            yield return new object[] { eventCall, $"emit {eventName}()" };

            modifierAppliance.Parameters = parameters;
            yield return new object[] { modifierAppliance, $"{modifierName}()" };

            var vName1 = "vName";
            var v1 = new Variable()
            {
                Name = vName1,
                Type = SolidityType.Bool
            };

            parameters.Parameters.Add(v1);
            yield return new object[] { eventCall, $"emit {eventName}({vName1})" };
            yield return new object[] { functionCall, $"{funcName}({vName1})" };
            yield return new object[] { modifierAppliance, $"{modifierName}({vName1})" };

            var vName2 = "vName2";
            var v2 = new Variable()
            {
                Name = vName2,
                Type = SolidityType.Bool
            };

            parameters.Parameters.Add(v2);
            yield return new object[] { eventCall, $"emit {eventName}({vName1}, {vName2})" };
            yield return new object[] { functionCall, $"{funcName}({vName1}, {vName2})" };
            yield return new object[] { modifierAppliance, $"{modifierName}({vName1}, {vName2})" };
        }
    }
}