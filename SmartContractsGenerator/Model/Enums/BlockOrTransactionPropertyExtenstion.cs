using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Enums
{
    public static class BlockOrTransactionPropertyExtenstion
    {
        private static readonly Dictionary<BlockOrTransactionProperty, string> AppropriateCode = new Dictionary<BlockOrTransactionProperty, string>()
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

        public static string GenerateCode(this BlockOrTransactionProperty property)
        {
            return AppropriateCode[property];
        }
    }
}
