using SmartContractsGenerator.Model.AbstractPatterns;
using System;

namespace SmartContractsGeneratorTests.Model.Helpers
{
    class InstructionsListMockCreator : IDisposable
    {
        private readonly MocksHolder mocksHolder = new MocksHolder();

        public void Dispose()
        {
            mocksHolder.Dispose();
        }

        public InstructionsList PrepareMock(string expectedCode, bool containsOnlyIf, bool containsAnyElement)
        {
            var mock = mocksHolder.GetMock();

            mock.Mock<InstructionsList>()
                .Setup(x => x.GenerateCode())
                .Returns(expectedCode);

            mock.Mock<InstructionsList>()
                .Setup(x => x.ContainsOnlyIf())
                .Returns(containsOnlyIf);

            mock.Mock<InstructionsList>()
                .Setup(x => x.Any())
                .Returns(containsAnyElement);

            var preparedMock = mock.Create<InstructionsList>();

            return preparedMock;
        }
    }
}
