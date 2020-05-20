using System;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Enums
{
    public static class OperationOperatorExtension
    {
        private static readonly Dictionary<OperationOperator, string> AppropriateCode = new Dictionary<OperationOperator, string>()
        {
            { OperationOperator.AND, "&&" },
            { OperationOperator.Divide, "/" },
            { OperationOperator.Equals, "==" },
            { OperationOperator.Greater, ">" },
            { OperationOperator.GreaterEqual, ">=" },
            { OperationOperator.Less, "<" },
            { OperationOperator.LessEqual, "<=" },
            { OperationOperator.Minus, "-" },
            { OperationOperator.Modulo, "%" },
            { OperationOperator.Multiply, "*" },
            { OperationOperator.Negation, "!" },
            { OperationOperator.NotEquals, "!=" },
            { OperationOperator.OR, "||" },
            { OperationOperator.Plus, "+" },
            { OperationOperator.XOR, "^" }
        };

        public static string GenerateCode(this OperationOperator type)
        {
            return AppropriateCode[type];
        }

        private static readonly HashSet<OperationOperator> MathOperators = new HashSet<OperationOperator>()
        {
            OperationOperator.Divide,
            OperationOperator.Minus,
            OperationOperator.Modulo,
            OperationOperator.Multiply,
            OperationOperator.Plus
        };

        private static readonly HashSet<OperationOperator> LogicOperators = new HashSet<OperationOperator>()
        {
            OperationOperator.AND,
            OperationOperator.Equals,
            OperationOperator.Greater,
            OperationOperator.GreaterEqual,
            OperationOperator.Less,
            OperationOperator.LessEqual,
            OperationOperator.Negation,
            OperationOperator.NotEquals,
            OperationOperator.OR,
            OperationOperator.XOR
        };

        private static readonly HashSet<OperationOperator> UnaryOperators = new HashSet<OperationOperator>()
        {
            OperationOperator.Negation
        };

        public static bool IsUnaryOperator(this OperationOperator arithmeticOperator)
        {
            return UnaryOperators.Contains(arithmeticOperator);
        }
        public static bool IsLogicOperator(this OperationOperator arithmeticOperator)
        {
            return LogicOperators.Contains(arithmeticOperator);
        }

        public static bool IsMathOperator(this OperationOperator arithmeticOperator)
        {
            return MathOperators.Contains(arithmeticOperator);
        }
    }
}