using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Enums.Tests
{
    [TestClass()]
    public class OperationOperatorExtensionTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(OperationOperator operationOperator, string expected, bool isUnary, bool isLogic, bool isMath)
        {
            Assert.AreEqual(expected, operationOperator.GenerateCode());
            Assert.AreEqual(isUnary, operationOperator.IsUnaryOperator());
            Assert.AreEqual(isLogic, operationOperator.IsLogicOperator());
            Assert.AreEqual(isMath, operationOperator.IsMathOperator());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            yield return new object[] { OperationOperator.AND, "&&", false, true, false };
            yield return new object[] { OperationOperator.Divide, "/", false, false, true };
            yield return new object[] { OperationOperator.Equals, "==", false, true, false };
            yield return new object[] { OperationOperator.Minus, "-", false, false, true };
            yield return new object[] { OperationOperator.Modulo, "%", false, false, true };
            yield return new object[] { OperationOperator.Multiply, "*", false, false, true };
            yield return new object[] { OperationOperator.Negation, "!", true, true, false };
            yield return new object[] { OperationOperator.OR, "||", false, true, false };
            yield return new object[] { OperationOperator.Plus, "+", false, false, true };
            yield return new object[] { OperationOperator.XOR, "^", false, true, false };
            yield return new object[] { OperationOperator.NotEquals, "!=", false, true, false };
            yield return new object[] { OperationOperator.Less, "<", false, true, false };
            yield return new object[] { OperationOperator.LessEqual, "<=", false, true, false };
            yield return new object[] { OperationOperator.Greater, ">", false, true, false };
            yield return new object[] { OperationOperator.GreaterEqual, ">=", false, true, false };
        }
    }
}