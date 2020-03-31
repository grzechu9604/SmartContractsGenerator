using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Enums.Tests
{
    [TestClass()]
    public class OperationOperatorExtensionTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(OperationOperator operationOperator, string expected)
        {
            Assert.AreEqual(expected, operationOperator.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            yield return new object[] { OperationOperator.AND, "&&" };
            yield return new object[] { OperationOperator.Divide, "/" };
            yield return new object[] { OperationOperator.Equals, "==" };
            yield return new object[] { OperationOperator.Minus, "-" };
            yield return new object[] { OperationOperator.Modulo, "%" };
            yield return new object[] { OperationOperator.Multiply, "*" };
            yield return new object[] { OperationOperator.Negation, "!" };
            yield return new object[] { OperationOperator.OR, "||" };
            yield return new object[] { OperationOperator.Plus, "+" };
            yield return new object[] { OperationOperator.XOR, "^" };
            yield return new object[] { OperationOperator.NotEquals, "!=" };
        }
    }
}