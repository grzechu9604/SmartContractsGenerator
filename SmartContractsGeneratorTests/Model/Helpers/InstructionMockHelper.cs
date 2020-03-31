using SmartContractsGenerator.Interfaces;
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

        public IInstruction PrepareMock(string expectedCode)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<IInstruction>()
                .Setup(x => x.GenerateCode())
                .Returns(expectedCode);

            return mock.Create<IInstruction>();
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
