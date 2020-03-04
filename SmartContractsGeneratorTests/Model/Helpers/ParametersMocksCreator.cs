using Autofac.Extras.Moq;
using SmartContractsGenerator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class ParametersMocksCreator
    {
        private readonly List<AutoMock> _mocks = new List<AutoMock>();

        public void DisposeMocks()
        {
            _mocks.ForEach(m => m.Dispose());
        }

        public Parameter PrepareParameterMock(string expected)
        {
            var mock = AutoMock.GetLoose();
            _mocks.Add(mock);

            mock.Mock<Parameter>()
                .Setup(x => x.GenerateCode())
                .Returns(expected);

            var preparedMock = mock.Create<Parameter>();

            return preparedMock;
        }
    }
}
