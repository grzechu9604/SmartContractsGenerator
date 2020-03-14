using System;
using System.Collections.Generic;
using System.Text;

namespace SmartContractsGenerator.Model
{
    public class ConstantValue
    {
        public string Value { get; set; }

        public string GenerateUsageCode() => Value;
    }
}
