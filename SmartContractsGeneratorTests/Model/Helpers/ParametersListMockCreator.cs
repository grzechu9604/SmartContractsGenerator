using SmartContractsGenerator.Model.AbstractPatterns;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class ParametersListMockCreator
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public ParametersList PrepareMock(string expected)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<ParametersList>()
                .Setup(x => x.GenerateCode())
                .Returns(expected);

            var preparedMock = mock.Create<ParametersList>();

            return preparedMock;
        }
    }
}
