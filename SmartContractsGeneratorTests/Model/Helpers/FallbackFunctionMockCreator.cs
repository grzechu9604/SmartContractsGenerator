using SmartContractsGenerator.Model.AbstractPatterns;
using SmartContractsGenerator.Model.SpecialFunctions;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class FallbackFunctionMockCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public FallbackFunction PrepareMock(string expectedGenerateCode, Indentation indentation)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<FallbackFunction>()
                .Setup(x => x.GenerateCode(indentation))
                .Returns(expectedGenerateCode);

            var preparedMock = mock.Create<FallbackFunction>();

            return preparedMock;
        }
    }
}
