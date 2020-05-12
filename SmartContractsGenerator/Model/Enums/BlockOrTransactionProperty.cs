namespace SmartContractsGenerator.Model.Enums
{
    public enum BlockOrTransactionProperty
    {
        /// <summary>
        /// address payable - current Block miner’s address
        /// </summary>
        BlockCoinbase = 0,
        /// <summary>
        /// uint - current Block difficulty
        /// </summary>
        BlockDifficulty = 1,
        /// <summary>
        /// uint - current Block gaslimit
        /// </summary>
        BlockGaslimit = 2,
        /// <summary>
        /// uint - current Block number
        /// </summary>
        BlockNumber = 3,
        /// <summary>
        /// uint - current Block timestamp as seconds since unix epoch
        /// </summary>
        BlockTimestamp = 4,
        /// <summary>
        /// uint256 - remaining gas
        /// </summary>
        Gasleft = 5,
        /// <summary>
        /// bytes calldata - complete calldata
        /// </summary>
        MsgData = 6,
        /// <summary>
        /// address payable - sender of the message (current call)
        /// </summary>
        MsgSender = 7,
        /// <summary>
        /// bytes4 - first four bytes of the calldata (i.e. function identifier)
        /// </summary>
        MsgSig = 8,
        /// <summary>
        /// uint - number of wei sent with the message
        /// </summary>
        MsgValue = 9,
        /// <summary>
        /// uint - current Block timestamp (alias for Block.timestamp)
        /// </summary>
        Now = 10,
        /// <summary>
        /// uint - gas price of the transaction
        /// </summary>
        TxGasprice = 11,
        /// <summary>
        /// address payable - sender of the transaction (full call chain)
        /// </summary>
        TxOrigin = 12
    }
}
