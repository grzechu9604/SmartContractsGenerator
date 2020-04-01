using SmartContractsGenerator.Model;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class AssignmentMockCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public Assignment PrepareMock(string expectedCode)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<Assignment>()
                .Setup(x => x.GenerateCode())
                .Returns(expectedCode);

            var preparedMock = mock.Create<Assignment>();

            return preparedMock;
        }
    }
}
