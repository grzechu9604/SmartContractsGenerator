using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGeneratorTests.Model.Helpers;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class ContractLoopTests
    {
        private static readonly ConditionCodeMockCreator conditionCodeMockCreator = new ConditionCodeMockCreator();
        private static readonly InstructionsListMockCreator instructionsListMockCreator = new InstructionsListMockCreator();
        private static readonly InstructionMockHelper instructionMockHelper = new InstructionMockHelper();
        private static readonly AssignmentMockCreator assignmentMockCreator = new AssignmentMockCreator();

        [TestCleanup]
        public void Cleanup()
        {
            conditionCodeMockCreator.Dispose();
            instructionsListMockCreator.Dispose();
            instructionMockHelper.Dispose();
            assignmentMockCreator.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyContractLoopTest()
        {
            new ContractLoop().GenerateCode(new Indentation());
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyContractLoopTest2()
        {
            new ContractLoop()
            {
                InitialAssignment = new Assignment()
            }.GenerateCode(new Indentation());
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyContractLoopTest3()
        {
            new ContractLoop()
            {
                InitialAssignment = new Assignment(),
                BreakCondition = new Condition()
            }.GenerateCode(new Indentation());
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(ContractLoop loop, Indentation indentation, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(loop != null);
            Assert.AreEqual(expected, loop.GenerateCode(indentation));
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            var indentationLevelZero = new Indentation();
            string breakConditionCode1 = "BREAK CONDITION1";
            string instructionCode1 = "\tINSTRUCTION CODE1";
            string initialDeclaration1 = "INITIAL DECLARATION CODE1";
            string stepInstruction1 = "STEP INSTRUCTION CODE1";
            yield return new object[] { GetMockedLoop(breakConditionCode1, instructionCode1, initialDeclaration1, stepInstruction1, indentationLevelZero), indentationLevelZero, $"for ({initialDeclaration1}; {breakConditionCode1}; {stepInstruction1}) {{\n{instructionCode1}\n}}" };
            
            string breakConditionCode2 = "BREAK CONDITION2";
            string instructionCode2 = "\tINSTRUCTION CODE2";
            string initialDeclaration2 = "INITIAL DECLARATION CODE2";
            string stepInstruction2 = "STEP INSTRUCTION CODE2";
            yield return new object[] { GetMockedLoop(breakConditionCode2, instructionCode2, initialDeclaration2, stepInstruction2, indentationLevelZero), indentationLevelZero, $"for ({initialDeclaration2}; {breakConditionCode2}; {stepInstruction2}) {{\n{instructionCode2}\n}}" };

            var indentationLevelOne = indentationLevelZero.GetIndentationWithIncrementedLevel();
            string instructionCodeWithLevelTwo = "\t\tINSTRUCTION CODE1";
            yield return new object[] { GetMockedLoop(breakConditionCode1, instructionCodeWithLevelTwo, initialDeclaration1, stepInstruction1, indentationLevelOne), indentationLevelOne, $"for ({initialDeclaration1}; {breakConditionCode1}; {stepInstruction1}) {{\n{instructionCodeWithLevelTwo}\n\t}}" };
        }

        static ContractLoop GetMockedLoop(string breakConditionCode, string instructionCode, string initialDeclaration, string stepInstruction, Indentation indentation)
        {
            return new ContractLoop()
            {
                BreakCondition = conditionCodeMockCreator.PrepareMock(breakConditionCode),
                Instructions = instructionsListMockCreator.PrepareMock(instructionCode, false, true, indentation.GetIndentationWithIncrementedLevel()),
                InitialAssignment = assignmentMockCreator.PrepareMock(initialDeclaration),
                StepInstruction = instructionMockHelper.PrepareOneLineInstructionMock(stepInstruction)
            };
        }
    }
}