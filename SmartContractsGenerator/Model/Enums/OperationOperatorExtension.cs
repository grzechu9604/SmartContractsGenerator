using System;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Enums
{
    public static class OperationOperatorExtension
    {
        public static readonly HashSet<OperationOperator> UnaryOperators = new HashSet<OperationOperator>()
        {
            OperationOperator.Negation
        };
        public static string GenerateCode(this OperationOperator @operator)
        {
            switch (@operator)
            {
                case OperationOperator.Plus:
                    return "+";
                case OperationOperator.Minus:
                    return "-";
                case OperationOperator.Modulo:
                    return "%";
                case OperationOperator.Divide:
                    return "/";
                case OperationOperator.Multiply:
                    return "*";
                case OperationOperator.Negation:
                    return "!";
                case OperationOperator.OR:
                    return "||";
                case OperationOperator.AND:
                    return "&&";
                case OperationOperator.XOR:
                    return "^";
                case OperationOperator.Equals:
                    return "==";
                case OperationOperator.NotEquals:
                    return "!=";
            }

            throw new InvalidOperationException("Invalid operator value!");
        }

        public static bool IsUnaryOperator(this OperationOperator arithmeticOperator)
        {
            return UnaryOperators.Contains(arithmeticOperator);
        }
    }
}