using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.AbstractPatterns;
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

        public ContractFunction PrepareMock(string expectedGenerateCode, string expectedGenerateCallCode, Indentation indentation)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<ContractFunction>()
                .Setup(x => x.GenerateCode(indentation))
                .Returns(expectedGenerateCode);

            mock.Mock<ContractFunction>()
                .Setup(x => x.GenerateCallCode())
                .Returns(expectedGenerateCallCode);

            var preparedMock = mock.Create<ContractFunction>();

            return preparedMock;
        }
    }
}
