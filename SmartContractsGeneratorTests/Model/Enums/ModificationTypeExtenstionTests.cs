using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Enums.Tests
{
    [TestClass()]
    public class ModificationTypeExtenstionTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(ModificationType modificationType, string expected)
        {
            Assert.AreEqual(expected, modificationType.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            var appropriateCode = new Dictionary<ModificationType, string>()
            {
                { ModificationType.None, null },
                { ModificationType.View, "view" },
                { ModificationType.Pure, "pure" }
            };

            foreach (var entry in appropriateCode)
            {
                yield return new object[] { entry.Key, entry.Value };
            }
        }
    }
}