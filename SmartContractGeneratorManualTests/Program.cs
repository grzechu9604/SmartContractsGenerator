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
            var boolType = "bool";
            var intType = "int256";

            var propertyName = "PropertyName1";
            var propertyName2 = "PropertyName2";
            var propertyName3 = "PropertyName3";

            var pr1 = new ContractProperty()
            {
                Variable = PrepareVariable(propertyName, boolType),
                Visibility = Visibility.Public
            };

            var pr2 = new ContractProperty()
            {
                Variable = PrepareVariable(propertyName2, intType),
                Visibility = Visibility.Private
            };

            var pr3 = new ContractProperty()
            {
                Variable = PrepareVariable(propertyName3, boolType),
                Visibility = Visibility.Public
            };

            var properties = new List<ContractProperty>() { pr1, pr2, pr3 };

            var eventName = "EventName1";
            var eventName2 = "EventName2";
            var eventName3 = "EventName3";

            var epl = new ParametersList()
            {
                Parameters = new List<Variable>() { PrepareVariable("ep1", boolType), PrepareVariable("ep2", intType) }
            };

            var epl2 = new ParametersList()
            {
                Parameters = new List<Variable>() { PrepareVariable("ep1", boolType) }
            };

            var epl3 = new ParametersList()
            {
                Parameters = new List<Variable>() { PrepareVariable("ep1", intType) }
            };

            var e1 = new ContractEvent()
            {
                Name = eventName,
                Parameters = epl
            };

            var e2 = new ContractEvent()
            {
                Name = eventName2,
                Parameters = epl2
            };

            var e3 = new ContractEvent()
            {
                Name = eventName3,
                Parameters = epl3
            };

            var events = new List<ContractEvent>() { e1, e2, e3 };

            var name1 = "_p";
            var name2 = "_q";
            var name3 = "_r";
            var name4 = "v";
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
                Destination = pr2,
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

            var trueInstructions = new InstructionsList();
            var falseInstructions = new InstructionsList();

            trueInstructions.AppendInstruction(
                new FunctionCall() { FunctionToCall = function, Parameters = pl2 }
                );

            var ecpl = new ParametersList()
            {
                Parameters = new List<Variable>() { p1 }
            };
            falseInstructions.AppendInstruction(
                new EventCall() { EventToCall = e2, Parameters = ecpl }
                );

            var newFInstructions = new InstructionsList();
            var condOp = new Operation()
            {
                LeftSide = p1,
                Operator = OperationOperator.Negation
            };
            var cond = new Condition() { ConditionOperation = condOp };
            var ifStatement = new IfStatement()
            {
                Condition = cond,
                TrueInstructions = trueInstructions,
                FalseInstructions = falseInstructions
            };

            newFInstructions.AppendInstruction(ifStatement);

            var loopVariable = PrepareVariable("loopVariable", intType);
            var declaration = new Declaration()
            {
                Variable = loopVariable
            };
            var assignment = new Assignment()
            {
                Destination = declaration,
                Source = new ConstantValue() { Value = "0" }
            };

            var condOperation = new Operation()
            {
                LeftSide = loopVariable,
                Operator = OperationOperator.NotEquals,
                RightSide = new ConstantValue() { Value = "1" }
            };

            var breakCondition = new Condition()
            {
                ConditionOperation = condOperation
            };

            var stepOp = new Operation()
            {
                LeftSide = loopVariable,
                Operator = OperationOperator.Plus,
                RightSide = new ConstantValue() { Value = "1" }
            };

            var stepInstruction = new Assignment()
            {
                Destination = loopVariable,
                Source = stepOp
            };

            var loopInstructions = new InstructionsList();
            var ple = new ParametersList()
            {
                Parameters = new List<Variable>() { loopVariable }
            };
            loopInstructions.AppendInstruction(
                new EventCall()
                {
                    EventToCall = e3,
                    Parameters = ple
                }
                );

            var loop = new ContractLoop()
            {
                InitialAssignment = assignment,
                BreakCondition = breakCondition,
                StepInstruction = stepInstruction,
                Instructions = loopInstructions
            };

            newFInstructions.AppendInstruction(loop);

            var function3 = new ContractFunction()
            {
                Name = "NewFunction",
                Visibility = Visibility.Public,
                Parameters = pl2,
                Instructions = newFInstructions
            };

            var functions = new List<ContractFunction>()
            {
                function, function2, function3
            };

            string contractName = "TestContract";
            Contract contract = new Contract()
            {
                Name = contractName,
                Constructor = c,
                Functions = functions,
                Events = events,
                Properties = properties
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
