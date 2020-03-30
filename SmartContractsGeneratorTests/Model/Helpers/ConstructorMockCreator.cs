using SmartContractsGenerator.Model;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class ConstructorMockCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public Constructor PrepareMock(string expectedCode)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<Constructor>()
                .Setup(x => x.GenerateCode())
                .Returns(expectedCode);

            var preparedMock = mock.Create<Constructor>();

            return preparedMock;
        }
    }
}
