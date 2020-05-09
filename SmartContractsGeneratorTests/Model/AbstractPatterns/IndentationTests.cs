using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.AbstractPatterns.Tests
{
    [TestClass()]
    public class IndentationTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(Indentation indentation, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(indentation != null);
            Assert.AreEqual(expected, indentation.GenerateCode());
        }
        static IEnumerable<object[]> GetDataForTests()
        {
            var indentation = new Indentation();
            yield return new object[] { indentation, string.Empty };

            for (int i = 1; i < 10; i++)
            {
                indentation.IncrementIndentationLevel();
                yield return new object[] { indentation, new string('\t', i) };
            }
        }
    }
}