using SmartContractsGenerator.Model;
using SmartContractsGenerator.Model.AbstractPatterns;
using System;
using System.Collections.Generic;

namespace SmartContractsGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var name1 = "Name1";
            var type1 = "Type1";

            var p1 = new Parameter()
            {
                Name = name1,
                Type = type1
            };

            var name2 = "Name2";
            var type2 = "Type2";
            var p2 = new Parameter()
            {
                Name = name2,
                Type = type2
            };

            var name3 = "Name3";
            var type3 = "Type3";
            var p3 = new Parameter()
            {
                Name = name3,
                Type = type3
            };

            var pl = new ParametersList()
            {
                Parameters = new List<Parameter>() { p1, p2, p3 }
            };

            var cModifier = "public";
            var c = new Constructor()
            {
                Modifier = cModifier,
                Parameters = pl
            };

            string contractName = "Test";
            Contract contract = new Contract()
            {
                Name = contractName,
                Constructor = c
            };

            Console.WriteLine(contract.GenerateCode());
        }
    }
}
