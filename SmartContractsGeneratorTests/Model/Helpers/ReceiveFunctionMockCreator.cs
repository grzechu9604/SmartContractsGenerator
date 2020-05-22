using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Model.SpecialFunctions;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class ReceiveFunctionMockCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public ReceiveFunction PrepareMock(string expectedGenerateCode, Indentation indentation)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<ReceiveFunction>()
                .Setup(x => x.GenerateCode(indentation))
                .Returns(expectedGenerateCode);

            var preparedMock = mock.Create<ReceiveFunction>();

            return preparedMock;
        }
    }
}
