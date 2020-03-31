using SmartContractsGenerator.Model;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class FunctionMockCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public ContractFunction PrepareMock(string expectedGenerateCode, string expectedGenerateCallCode)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<ContractFunction>()
                .Setup(x => x.GenerateCode())
                .Returns(expectedGenerateCode);

            mock.Mock<ContractFunction>()
                .Setup(x => x.GenerateCallCode())
                .Returns(expectedGenerateCallCode);

            var preparedMock = mock.Create<ContractFunction>();

            return preparedMock;
        }
    }
}
