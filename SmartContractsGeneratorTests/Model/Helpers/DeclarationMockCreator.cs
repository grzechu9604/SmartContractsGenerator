using SmartContractsGenerator.Model;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class DeclarationMockCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public Declaration PrepareMock(string expectedCode)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<Declaration>()
                .Setup(x => x.GenerateCode())
                .Returns(expectedCode);

            var preparedMock = mock.Create<Declaration>();

            return preparedMock;
        }
    }
}
