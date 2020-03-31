using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
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
        private static readonly DeclarationMockCreator declarationMockCreator = new DeclarationMockCreator();

        [TestCleanup]
        public void Cleanup()
        {
            conditionCodeMockCreator.Dispose();
            instructionsListMockCreator.Dispose();
            instructionMockHelper.Dispose();
            declarationMockCreator.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]

        public void EmptyContractLoopTest()
        {
            new ContractLoop().GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyContractLoopTest2()
        {
            new ContractLoop()
            {
                InitialDeclaration = new Declaration()
            }.GenerateCode();
        }

        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyContractLoopTest3()
        {
            new ContractLoop()
            {
                InitialDeclaration = new Declaration(),
                BreakCondition = new Condition()
            }.GenerateCode();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(ContractLoop loop, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(loop != null);
            Assert.AreEqual(expected, loop.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            string breakConditionCode1 = "BREAK CONDITION1";
            string instructionCode1 = "INSTRUCTION CODE1";
            string initialDeclaration1 = "INITIAL DECLARATION CODE1";
            string stepInstruction1 = "STEP INSTRUCTION CODE1";
            yield return new object[] { GetMockedLoop(breakConditionCode1, instructionCode1, initialDeclaration1, stepInstruction1), $"for ({initialDeclaration1}; {breakConditionCode1}; {stepInstruction1}) {{\n{instructionCode1}\n}}" };
            
            string breakConditionCode2 = "BREAK CONDITION2";
            string instructionCode2 = "INSTRUCTION CODE2";
            string initialDeclaration2 = "INITIAL DECLARATION CODE2";
            string stepInstruction2 = "STEP INSTRUCTION CODE2";
            yield return new object[] { GetMockedLoop(breakConditionCode2, instructionCode2, initialDeclaration2, stepInstruction2), $"for ({initialDeclaration2}; {breakConditionCode2}; {stepInstruction2}) {{\n{instructionCode2}\n}}" };

        }

        static ContractLoop GetMockedLoop(string breakConditionCode, string instructionCode, string initialDeclaration, string stepInstruction)
        {
            return new ContractLoop()
            {
                BreakCondition = conditionCodeMockCreator.PrepareMock(breakConditionCode),
                Instructions = instructionsListMockCreator.PrepareMock(instructionCode, false, true),
                InitialDeclaration = declarationMockCreator.PrepareMock(initialDeclaration),
                StepInstruction = instructionMockHelper.PrepareMock(stepInstruction)
            };
        }
    }
}