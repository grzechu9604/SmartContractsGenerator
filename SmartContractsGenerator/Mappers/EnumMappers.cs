using SmartContractsGenerator.Model.Enums;
using System;
using System.Linq;

namespace SmartContractsGenerator.Mappers
{
    class EnumMappers
    {
        private const int MinVisibilityValue = 0;
        private const int MaxVisibilityValue = 3;
        private const int MinOperationOperatorValue = 0;
        private const int MaxVOperationOperatorValue = 10;
        public static Visibility? MapBlocklyCodeToVisibility(string code)
        {
            if (!string.IsNullOrEmpty(code) && code.Length == 1 && char.IsDigit(code.First()))
            {
                var value = Convert.ToInt16(code);
                if (value >= MinVisibilityValue && value <= MaxVisibilityValue)
                {
                    return (Visibility)value;
                }
            }

            throw new InvalidOperationException($"Value {code} is invalid option for visibility code!");
        }

        public static OperationOperator? MapBlocklyCodeToOperationOperator(string code)
        {
            if (!string.IsNullOrEmpty(code) && (code.Length == 1 || code.Length == 2) && code.All(c => char.IsDigit(c)))
            {
                var value = Convert.ToInt16(code);
                if (value >= MinOperationOperatorValue && value < MaxVOperationOperatorValue)
                {
                    return (OperationOperator)value;
                }
            }

            throw new InvalidOperationException($"Value {code} is invalid option for visibility code!");
        }
    }
}
