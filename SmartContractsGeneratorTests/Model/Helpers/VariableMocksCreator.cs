using Autofac.Extras.Moq;
using SmartContractsGenerator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class VariableMocksCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public Variable PrepareMock(string expected)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<Variable>()
                .Setup(x => x.GenerateCode())
                .Returns(expected);

            var preparedMock = mock.Create<Variable>();

            return preparedMock;
        }
    }
}
