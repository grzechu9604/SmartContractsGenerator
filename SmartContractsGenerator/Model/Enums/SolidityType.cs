namespace SmartContractsGenerator.Model.Enums
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "It is enum of Solidity types. It must contain type name")]
    public enum SolidityType
    {
        Bool = 0,
        Int = 1,
        UInt = 2,
        Fixed = 3,
        UFixed = 4,
        Address = 5,
        AddressPayable = 6,
        String = 7
    }
}
