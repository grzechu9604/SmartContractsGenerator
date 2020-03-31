using SmartContractsGenerator.Model;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class EventMockCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public ContractEvent PrepareMock(string expectedGenerateCode, string expectedGenerateCallCode)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<ContractEvent>()
                .Setup(x => x.GenerateCode())
                .Returns(expectedGenerateCode);

            mock.Mock<ContractEvent>()
                .Setup(x => x.GenerateCallCode())
                .Returns(expectedGenerateCallCode);

            var preparedMock = mock.Create<ContractEvent>();

            return preparedMock;
        }
    }
}
