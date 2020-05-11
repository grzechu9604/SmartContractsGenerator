using SmartContractsGenerator.Interfaces;
using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.AbstractPatterns;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class InstructionMockHelper : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public IInstruction PrepareMock(string expectedCode, Indentation indentation)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<IfStatement>()
                .Setup(x => x.GenerateCode(indentation))
                .Returns(expectedCode);

            return mock.Create<IfStatement>();
        }

        public IOneLineInstruction PrepareOneLineInstructionMock(string expectedCode)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<IOneLineInstruction>()
                .Setup(x => x.GenerateCode())
                .Returns(expectedCode);

            return mock.Create<IOneLineInstruction>();
        }
    }
}
