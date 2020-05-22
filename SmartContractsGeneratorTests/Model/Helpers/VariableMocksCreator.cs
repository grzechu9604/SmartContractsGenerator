using SmartContractsGenerator.Model;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class VariableMocksCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public Variable PrepareMock(string expectedCode, string expectedDeclarationCode, bool pointStorageType)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<Variable>()
                .Setup(x => x.GenerateCode())
                .Returns(expectedCode);

            mock.Mock<Variable>()
                .Setup(x => x.GenerateDeclarationCode(pointStorageType))
                .Returns(expectedDeclarationCode);

            var preparedMock = mock.Create<Variable>();

            return preparedMock;
        }
    }
}
