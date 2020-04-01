using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Model.Enums;
using System;
using System.Collections.Generic;

namespace SmartContractGeneratorManualTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var name1 = "_p";
            var name2 = "_q";
            var name3 = "_r";
            var name4 = "v";

            var boolType = "bool";
            var intType = "int256";

            var p1 = PrepareVariable(name1, boolType);
            var p2 = PrepareVariable(name2, boolType);
            var p3 = PrepareVariable(name3, boolType);
            var v = PrepareVariable(name4, boolType);

            var pl = new ParametersList()
            {
                Parameters = new List<Variable>() { p1, p2 }
            };

            var pl2 = new ParametersList()
            {
                Parameters = new List<Variable>() { p1, p2, p3 }
            };

            var d = new Declaration()
            {
                Variable = v
            };

            var op = new Operation()
            {
                LeftSide = p1,
                Operator = OperationOperator.OR,
                RightSide = p2
            };

            var instruction = new Assignment()
            {
                Destination = d,
                Source = op
            };

            var ao = new Operation()
            {
                LeftSide = v,
                Operator = OperationOperator.Negation
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

            var functionInstructions = new InstructionsList();
            functionInstructions.AppendInstruction(instruction);

            var aof = new Operation()
            {
                LeftSide = v,
                Operator = OperationOperator.AND,
                RightSide = p3
            };

            var instruction3 = new Assignment()
            {
                Destination = v,
                Source = aof
            };

            functionInstructions.AppendInstruction(instruction3);

            var function = new ContractFunction()
            {
                Name = "TestFunction",
                Visibility = Visibility.Public,
                Parameters = pl2,
                Instructions = functionInstructions
            };

            var fp1 = PrepareVariable(name1, intType);
            var fp2 = PrepareVariable(name2, intType);
            var fp3 = PrepareVariable(name3, intType);
            var fv = PrepareVariable(name4, intType);

            var fpl = new ParametersList()
            {
                Parameters = new List<Variable>() { fp1, fp2, fp3 }
            };

            var fd = new Declaration()
            {
                Variable = fv
            };

            var fop = new Operation()
            {
                LeftSide = p1,
                Operator = OperationOperator.Plus,
                RightSide = p2
            };

            var finstruction = new Assignment()
            {
                Destination = fd,
                Source = fop
            };

            var fao = new Operation()
            {
                LeftSide = v,
                Operator = OperationOperator.Multiply,
                RightSide = fp3
            };

            var finstruction2 = new Assignment()
            {
                Destination = fv,
                Source = fao
            };

            var finstructions = new InstructionsList();
            finstructions.AppendInstruction(finstruction);
            finstructions.AppendInstruction(finstruction2);

            var function2 = new ContractFunction()
            {
                Name = "TestFunction2",
                Visibility = Visibility.External,
                Parameters = fpl,
                Instructions = finstructions
            };

            var functions = new List<ContractFunction>()
            {
                function, function2
            };

            string contractName = "TestContract";
            Contract contract = new Contract()
            {
                Name = contractName,
                Constructor = c,
                Functions = functions
            };

            Console.WriteLine(contract.GenerateCode());
        }

        private static Variable PrepareVariable(string name, string type)
        {
            return new Variable()
            {
                Name = name,
                Type = type
            };
        }
    }
}
