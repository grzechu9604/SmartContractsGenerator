﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGeneratorTests.Model.Helpers;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class OperationTests
    {
        private static readonly VariableMocksCreator variableMocksCreator = new VariableMocksCreator();

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyOperationTest()
        {
            new Operation().GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyOperationTest1()
        {
            new Operation()
            {
                LeftSide = new ConstantValue() { Value = "1" }
            }.GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyOperationTest2()
        {
            new Operation()
            {
                LeftSide = new ConstantValue() { Value = "1" },
                Operator = Enums.OperationOperator.AND
            }.GenerateCode();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(Operation o, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(o != null);
            Assert.AreEqual(expected, o.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            var op1 = new Operation()
            {
                LeftSide = new ConstantValue() { Value = "true" },
                Operator = Enums.OperationOperator.Negation
            };

            yield return new object[] { op1, "!(true)" };

            var op2 = new Operation()
            {
                LeftSide = new ConstantValue() { Value = "1" },
                Operator = Enums.OperationOperator.Equals,
                RightSide = new ConstantValue() { Value = "1" }
            };
            yield return new object[] { op2, "(1) == (1)" };

            var op3 = new Operation()
            {
                LeftSide = op1,
                Operator = Enums.OperationOperator.OR,
                RightSide = op2
            };
            yield return new object[] { op3, "(!(true)) || ((1) == (1))" };

            var variableName = "VARIABLE_NAME";
            var op4 = new Operation()
            {
                LeftSide = op3,
                Operator = Enums.OperationOperator.AND,
                RightSide = variableMocksCreator.PrepareMock(variableName, null)
            };
            yield return new object[] { op4, $"((!(true)) || ((1) == (1))) && ({variableName})" };

            var op5 = new Operation()
            {
                LeftSide = op4,
                Operator = Enums.OperationOperator.Equals,
                RightSide = op4
            };
            yield return new object[] { op5, $"(((!(true)) || ((1) == (1))) && ({variableName})) == (((!(true)) || ((1) == (1))) && ({variableName}))" };
        }
    }
}