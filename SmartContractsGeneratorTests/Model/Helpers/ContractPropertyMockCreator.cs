using SmartContractsGenerator.Model;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class ContractPropertyMockCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public ContractProperty PrepareMock(string expectedGenerateCode, string expectedGenerateDeclarationCode)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<ContractProperty>()
                .Setup(x => x.GenerateCode())
                .Returns(expectedGenerateCode);

            mock.Mock<ContractProperty>()
                .Setup(x => x.GenerateDeclarationCode())
                .Returns(expectedGenerateDeclarationCode);

            var preparedMock = mock.Create<ContractProperty>();

            return preparedMock;
        }
    }
}
