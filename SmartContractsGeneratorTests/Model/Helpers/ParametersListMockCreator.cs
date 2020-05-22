using SmartContractsGenerator.Model.AbstractPatterns;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class ParametersListMockCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public ParametersList PrepareMock(string expected, bool hasAnyParameter, bool pointStorageType)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<ParametersList>()
                .Setup(x => x.GenerateCode(pointStorageType))
                .Returns(expected);

            mock.Mock<ParametersList>()
                .Setup(x => x.AnyParameter())
                .Returns(hasAnyParameter);

            var preparedMock = mock.Create<ParametersList>();

            return preparedMock;
        }
    }
}
