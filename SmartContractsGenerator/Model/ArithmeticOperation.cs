using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model.Enums;

namespace SmartContractsGenerator.Model
{
    public class ArithmeticOperation : IAssignable
    {
        public IAssignable LeftSide { get; set; }
        public IAssignable RightSide { get; set; }

        public ArithmeticOperator? Operator { get; set; }

        public string GenerateCode()
        {
            if (LeftSide == null)
            {
                throw new MissingMandatoryElementException("LeftSide is mandatory in ArithmeticOperation");
            }

            if (Operator == null)
            {
                throw new MissingMandatoryElementException("Operator is mandatory in ArithmeticOperation");
            }

            if (Operator.Value.IsUnaryOperator())
            {
                return $"{Operator.Value.GenerateCode()}{LeftSide.GenerateCode()}";
            }

            if (RightSide == null)
            {
                throw new MissingMandatoryElementException("RightSide is mandatory in ArithmeticOperation with non-unary operator");
            }

            return $"{LeftSide.GenerateCode()} {Operator.Value.GenerateCode()} {RightSide.GenerateCode()}";
        }
    }
}
