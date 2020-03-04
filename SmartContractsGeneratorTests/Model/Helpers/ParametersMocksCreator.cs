﻿using Autofac.Extras.Moq;
using SmartContractsGenerator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class ParametersMocksCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public Parameter PrepareMock(string expected)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<Parameter>()
                .Setup(x => x.GenerateCode())
                .Returns(expected);

            var preparedMock = mock.Create<Parameter>();

            return preparedMock;
        }
    }
}
