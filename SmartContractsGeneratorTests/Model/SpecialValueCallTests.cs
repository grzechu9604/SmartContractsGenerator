using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartContractsGenerator.Exceptions;
using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model.Tests
{
    [TestClass()]
    public class SpecialValueCallTests
    {
        [TestMethod()]
        [ExpectedException(typeof(MissingMandatoryElementException))]
        public void EmptyAssignmentTest()
        {
            new SpecialValueCall().GenerateCode();
        }

        [TestMethod]
        [DynamicData(nameof(GetDataForGenerateCodeTest), DynamicDataSourceType.Method)]
        public void GenerateCodeTest(SpecialValueCall svc, string expected)
        {
            System.Diagnostics.Contracts.Contract.Requires(svc != null);
            Assert.AreEqual(expected, svc.GenerateCode());
        }

        static IEnumerable<object[]> GetDataForGenerateCodeTest()
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
                yield return new object[] { new SpecialValueCall() { PropertyToCall = entry.Key }, entry.Value };
            }
        }
    }
}