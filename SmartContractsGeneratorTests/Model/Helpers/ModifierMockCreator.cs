using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.AbstractPatterns;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class ModifierMockCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public Modifier PrepareMock(string expectedGenerateCode, Indentation indentation)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<Modifier>()
                .Setup(x => x.GenerateCode(indentation))
                .Returns(expectedGenerateCode);

            var preparedMock = mock.Create<Modifier>();

            return preparedMock;
        }
    }
}
