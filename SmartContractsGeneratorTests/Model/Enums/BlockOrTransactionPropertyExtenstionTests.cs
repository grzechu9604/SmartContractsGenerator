using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Enums.Tests
{
    [TestClass()]
    public class BlockOrTransactionPropertyExtenstionTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDataForTests), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(BlockOrTransactionProperty property, string expected)
        {
            Assert.AreEqual(expected, property.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForTests()
        {
            var appropriateCode = new Dictionary<BlockOrTransactionProperty, string>()
            {
                { BlockOrTransactionProperty.BlockCoinbase, "block.coinbase" },
                { BlockOrTransactionProperty.BlockDifficulty, "block.difficulty" },
                { BlockOrTransactionProperty.BlockGaslimit, "block.gaslimit" },
                { BlockOrTransactionProperty.BlockNumber, "block.number" },
                { BlockOrTransactionProperty.BlockTimestamp, "block.timestamp" },
                { BlockOrTransactionProperty.Gasleft, "gasleft()" },
                { BlockOrTransactionProperty.MsgData, "msg.data" },
                { BlockOrTransactionProperty.MsgSender, "msg.sender" },
                { BlockOrTransactionProperty.MsgSig, "msg.sig" },
                { BlockOrTransactionProperty.MsgValue, "msg.value" },
                { BlockOrTransactionProperty.Now, "now" },
                { BlockOrTransactionProperty.TxGasprice, "tx.gasprice" },
                { BlockOrTransactionProperty.TxOrigin, "tx.origin" }
            };

            foreach (var entry in appropriateCode)
            {
                yield return new object[] { entry.Key, entry.Value };
            }
        }
    }
}