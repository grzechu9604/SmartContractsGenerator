using SmartContractsGenerator.Model.AbstractPatterns;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class CallingParametersListMockCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public CallingParametersList PrepareMock(string expected, bool hasAnyParameter)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<CallingParametersList>()
                .Setup(x => x.GenerateCode())
                .Returns(expected);

            mock.Mock<CallingParametersList>()
                .Setup(x => x.AnyParameter())
                .Returns(hasAnyParameter);

            var preparedMock = mock.Create<CallingParametersList>();

            return preparedMock;
        }
    }
}
