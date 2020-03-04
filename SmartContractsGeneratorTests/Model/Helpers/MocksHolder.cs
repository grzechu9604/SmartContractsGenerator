using Autofac.Extras.Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class MocksHolder : IDisposable
    {
        private readonly List<AutoMock> _mocks = new List<AutoMock>();

        public void Dispose()
        {
            _mocks.ForEach(m => m.Dispose());
        }

        public void AddMock(AutoMock mock)
        {
            _mocks.Add(mock);
        }

        public AutoMock GetMock()
        {
            var mock = AutoMock.GetLoose();
            _mocks.Add(mock);
            return mock;
        }
    }
}
