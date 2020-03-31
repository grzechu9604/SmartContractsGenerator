using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SmartContractsGenerator.Validators.Tests
{
    [TestClass()]
    public class NameValidatorTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]

        public void IsValidNameTest(bool isValidName, string name)
        {
            Assert.AreEqual(isValidName, NameValidator.IsValidName(name));
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            yield return new object[] { true, "abc" }; 
            yield return new object[] { false, "1abc" }; 
            yield return new object[] { true, "a1bc" };
            yield return new object[] { true, "_abc" }; 
            yield return new object[] { true, "a_bc" };
            yield return new object[] { true, "Abc" };
            yield return new object[] { true, "Ab_c" };
            yield return new object[] { false, "@" };
            yield return new object[] { true, "_" };
            yield return new object[] { true, "_1" };
            yield return new object[] { false, "1_" };
            yield return new object[] { false, " " };
            yield return new object[] { false, "" };
        }
    }
}