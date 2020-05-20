using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Helpers.Tests
{
    [TestClass()]
    public class StringEscapersTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void EscapeStringTest(string input, string expected)
        {
            Assert.AreEqual(expected, SolidityStringsEscaper.EscapeString(input));
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            yield return new object[] { "abc", "\"abc\"" };
            yield return new object[] { "a\"bc", "\"a\\\"bc\"" };
            yield return new object[] { "a\"bc\"", "\"a\\\"bc\\\"\"" };
            yield return new object[] { "\"a\"bc", "\"\\\"a\\\"bc\"" };
            yield return new object[] { "\"a\"b\"c\"", "\"\\\"a\\\"b\\\"c\\\"\"" };
            yield return new object[] { @"a\bc", "\"a\\\\bc\"" };
            yield return new object[] { @"a\b\c", "\"a\\\\b\\\\c\"" };
            yield return new object[] { @"a\b\c\", "\"a\\\\b\\\\c\\\\\"" };
        }
    }
}