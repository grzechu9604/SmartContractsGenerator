using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.AbstractPatterns;
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

        public Constructor PrepareMock(string expectedCode, Indentation indentation)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<Constructor>()
                .Setup(x => x.GenerateCode(indentation))
                .Returns(expectedCode);

            var preparedMock = mock.Create<Constructor>();

            return preparedMock;
        }
    }
}
