using System;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Enums
{
    public static class ArithmeticOperatorExtension
    {
        public static readonly HashSet<ArithmeticOperator> UnaryOperators = new HashSet<ArithmeticOperator>()
        {
            ArithmeticOperator.Negation
        };
        public static string GenerateCode(this ArithmeticOperator arithmeticOperator)
        {
            switch (arithmeticOperator)
            {
                case ArithmeticOperator.Plus:
                    return "+";
                case ArithmeticOperator.Minus:
                    return "-";
                case ArithmeticOperator.Modulo:
                    return "%";
                case ArithmeticOperator.Divide:
                    return "/";
                case ArithmeticOperator.Multiply:
                    return "*";
                case ArithmeticOperator.Negation:
                    return "!";
                case ArithmeticOperator.OR:
                    return "||";
                case ArithmeticOperator.AND:
                    return "&&";
                case ArithmeticOperator.XOR:
                    return "^";
            }

            throw new InvalidOperationException("Invalid ArithmeticOperator value!");
        }

        public static bool IsUnaryOperator(this ArithmeticOperator arithmeticOperator)
        {
            return UnaryOperators.Contains(arithmeticOperator);
        }
    }
}