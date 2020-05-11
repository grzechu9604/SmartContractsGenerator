using System.Collections.Generic;

namespace SmartContractsGenerator.Model.Enums
{
    public static class ModificationTypeExtenstion
    {
        private static readonly Dictionary<ModificationType, string> AppropriateCode = new Dictionary<ModificationType, string>()
        {
            { ModificationType.None, null },
            { ModificationType.View, "view" },
            { ModificationType.Pure, "pure" }
        };


        public static string GenerateCode(this ModificationType modificationType)
        {
            return AppropriateCode[modificationType];
        }
    }
}
