﻿using SmartContractsGenerator.Model.Enums;
using System;
using System.Linq;

namespace SmartContractsGenerator.Mappers
{
    class EnumMappers
    {
        private const int MinVisibilityValue = 0;
        private const int MaxVisibilityValue = 3;
        private const int MinOperationOperatorValue = 0;
        private const int MaxOperationOperatorValue = 14;
        private const int MinModificationTypeValue = 0;
        private const int MaxModificationTypeValue = 2;
        private const int MinBlockOrTransactionPropertyValue = 0;
        private const int MaxBlockOrTransactionPropertyValue = 12;
        private const int MinSolidityType = 0;
        private const int MaxSolidityType = 7;

        public static Visibility MapBlocklyCodeToVisibility(string code)
        {
            if (!string.IsNullOrWhiteSpace(code) && code.Length == 1 && char.IsDigit(code.First()))
            {
                var value = Convert.ToInt16(code);
                if (value >= MinVisibilityValue && value <= MaxVisibilityValue)
                {
                    return (Visibility)value;
                }
            }

            throw new InvalidOperationException($"Value {code} is invalid option for visibility code!");
        }

        public static OperationOperator MapBlocklyCodeToMathOperationOperator(string code)
        {
            var op = MapBlocklyCodeToOperationOperator(code);
            if (op.IsMathOperator())
            {
                return op;
            }
            else
            {
                throw new InvalidOperationException("This operator isn't math operator!");
            }
        }

        public static OperationOperator MapBlocklyCodeToLogicOperationOperator(string code)
        {
            var op = MapBlocklyCodeToOperationOperator(code);
            if (op.IsLogicOperator())
            {
                return op;
            }
            else
            {
                throw new InvalidOperationException("This operator isn't logic operator!");
            }
        }

        public static OperationOperator MapBlocklyCodeToOperationOperator(string code)
        {
            if (!string.IsNullOrWhiteSpace(code) && (code.Length == 1 || code.Length == 2) && code.All(c => char.IsDigit(c)))
            {
                var value = Convert.ToInt16(code);
                if (value >= MinOperationOperatorValue && value <= MaxOperationOperatorValue)
                {
                    return (OperationOperator)value;
                }
            }

            throw new InvalidOperationException($"Value {code} is invalid option for operator code!");
        }

        public static ModificationType MapBlocklyCodeToModificationType(string code)
        {
            if (!string.IsNullOrWhiteSpace(code) && code.Length == 1 && code.All(c => char.IsDigit(c)))
            {
                var value = Convert.ToInt16(code);
                if (value >= MinModificationTypeValue && value <= MaxModificationTypeValue)
                {
                    return (ModificationType)value;
                }
            }

            throw new InvalidOperationException($"Value {code} is invalid option for ModificationType code!");
        }

        public static BlockOrTransactionProperty MapBlocklyCodeToBlockOrTransactionProperty(string code)
        {
            if (!string.IsNullOrWhiteSpace(code) && (code.Length == 1 || code.Length == 2) && code.All(c => char.IsDigit(c)))
            {
                var value = Convert.ToInt16(code);
                if (value >= MinBlockOrTransactionPropertyValue && value <= MaxBlockOrTransactionPropertyValue)
                {
                    return (BlockOrTransactionProperty)value;
                }
            }

            throw new InvalidOperationException($"Value {code} is invalid option for BlockOrTransactionProperty code!");
        }

        public static SolidityType MapBlocklyCodeToSolidityType(string code)
        {
            if (!string.IsNullOrWhiteSpace(code) && code.Length == 1 && code.All(c => char.IsDigit(c)))
            {
                var value = Convert.ToInt16(code);
                if (value >= MinSolidityType && value <= MaxSolidityType)
                {
                    return (SolidityType)value;
                }
            }

            throw new InvalidOperationException($"Value {code} is invalid option for SolidityType code!");
        }
    }
}
