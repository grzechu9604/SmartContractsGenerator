using SmartContractsGenerator.Model;
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

        public Modifier PrepareMock(string expectedGenerateCode, string expectedGenerateCallCode)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<Modifier>()
                .Setup(x => x.GenerateCode())
                .Returns(expectedGenerateCode);

            var preparedMock = mock.Create<Modifier>();

            return preparedMock;
        }
    }
}
