using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class BreakStatementTests
    {
        [TestMethod()]
        public void GenerateCodeTest()
        {
            Assert.AreEqual("break", new BreakStatement().GenerateCode());
        }
    }
}