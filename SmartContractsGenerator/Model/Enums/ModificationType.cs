namespace SmartContractsGenerator.Model.Enums
{
    public enum ModificationType
    {
        // Read/Write
        None = 0,
        // Read only
        View = 1,
        // neither read nor write
        Pure = 2
    }
}
