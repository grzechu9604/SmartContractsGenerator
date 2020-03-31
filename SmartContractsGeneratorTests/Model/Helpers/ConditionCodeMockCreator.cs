using SmartContractsGenerator.Model;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class ConditionCodeMockCreator :IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public Condition PrepareMock(string expectedCode)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<Condition>()
                .Setup(x => x.GenerateCode())
                .Returns(expectedCode);

            var preparedMock = mock.Create<Condition>();

            return preparedMock;
        }
    }
}
