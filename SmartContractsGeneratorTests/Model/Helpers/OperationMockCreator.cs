using SmartContractsGenerator.Model;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class OperationMockCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public Operation PrepareMock(string expectedCode)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<Operation>()
                .Setup(x => x.GenerateCode())
                .Returns(expectedCode);

            var preparedMock = mock.Create<Operation>();

            return preparedMock;
        }
    }
}
