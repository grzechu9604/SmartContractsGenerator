using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Model.AbstractPatterns;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class CallTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDataForGenerateCodeTest), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(Call c, string expected)
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
            var functionCall = new Call()
            {
                Callable = function
            };
            yield return new object[] { functionCall, $"{funcName}()" };

            string eventName = "eventName";
            var contractEvent = new ContractEvent()
            {
                Name = eventName
            };
            var eventCall = new Call()
            {
                Callable = contractEvent
            };
            yield return new object[] { eventCall, $"emit {eventName}()" };

            var variables = new List<Variable>();
            var parameters = new ParametersList()
            {
                Parameters = variables
            };

            functionCall.Parameters = parameters;
            yield return new object[] { functionCall, $"{funcName}()" };

            eventCall.Parameters = parameters;
            yield return new object[] { eventCall, $"emit {eventName}()" };

            var vName1 = "vName";
            var v1 = new Variable()
            {
                Name = vName1,
                Type = "TYPE"
            };

            parameters.Parameters.Add(v1);
            yield return new object[] { eventCall, $"emit {eventName}({vName1})" };
            yield return new object[] { functionCall, $"{funcName}({vName1})" };

            var vName2 = "vName2";
            var v2 = new Variable()
            {
                Name = vName2,
                Type = "TYPE"
            };

            parameters.Parameters.Add(v2);
            yield return new object[] { eventCall, $"emit {eventName}({vName1}, {vName2})" };
            yield return new object[] { functionCall, $"{funcName}({vName1}, {vName2})" };
        }
    }
}