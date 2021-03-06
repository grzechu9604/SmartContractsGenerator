﻿using SmartContractsGenerator.Interfaces;

namespace SmartContractsGenerator.Model.AbstractPatterns
{
    public class Indentation : ICodeGenerator
    {
        public Indentation()
        {
            IndentationLevel = 0;
        }

        private Indentation(int indentationLevel)
        {
            IndentationLevel = indentationLevel;
        }

        private readonly int IndentationLevel;

        public Indentation GetIndentationWithIncrementedLevel() => new Indentation(IndentationLevel + 1);

        public Indentation GetIndentationWithTheSameLevel() => new Indentation(IndentationLevel);


        public string GenerateCode()
        {
            return new string('\t', IndentationLevel);
        }

        public override bool Equals(object obj)
        {
            return obj is Indentation indentation && indentation.IndentationLevel == IndentationLevel;
        }
    }
}
