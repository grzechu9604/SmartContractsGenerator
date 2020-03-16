using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Model.Enums;
using System;
using System.Collections.Generic;

namespace SmartContractsGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var name1 = "_p";
            var type1 = "bool";

            var p1 = new Variable()
            {
                Name = name1,
                Type = type1
            };

            var name2 = "_q";
            var type2 = "bool";
            var p2 = new Variable()
            {
                Name = name2,
                Type = type2
            };

            var name3 = "_r";
            var type3 = "bool";
            var p3 = new Variable()
            {
                Name = name3,
                Type = type3
            };

            var pl = new ParametersList()
            {
                Parameters = new List<Variable>() { p1, p2, p3 }
            };

            var v = new Variable()
            {
                Name = "v",
                Type = "bool"
            };

            var d = new Declaration()
            {
                Variable = v
            };

            var op = new ArithmeticOperation()
            {
                LeftSide = p1,
                Operator = ArithmeticOperator.OR,
                RightSide = p2
            };

            var instruction = new Assignment()
            {
                Destination = d,
                Source = op
            };

            var ao = new ArithmeticOperation()
            {
                LeftSide = v,
                Operator = ArithmeticOperator.Negation
            };

            var instruction2 = new Assignment()
            {
                Destination = v,
                Source = ao
            };

            var instructions = new InstructionsList();
            instructions.AppendInstruction(instruction);
            instructions.AppendInstruction(instruction2);

            var c = new Constructor()
            {
                Visibility = Visibility.Public,
                Parameters = pl,
                Instructions = instructions
            };

            string contractName = "TestContract";
            Contract contract = new Contract()
            {
                Name = contractName,
                Constructor = c
            };

            Console.WriteLine(contract.GenerateCode());
        }
    }
}
