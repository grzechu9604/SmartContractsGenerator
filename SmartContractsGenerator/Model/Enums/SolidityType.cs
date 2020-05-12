namespace SmartContractsGenerator.Model.Enums
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "It is enum of Solidity types. It must contain type name")]
    public enum SolidityType
    {
        Bool = 1,
        Int = 2,
        UInt = 3,
        Fixed = 4,
        UFixed = 5,
        Address = 6,
        AddressPayable = 7,
        String = 8
    }
}
