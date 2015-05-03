using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public abstract class Generator
    {
        protected readonly LexicalAnalyzer tokenizer;
        protected readonly Code code;
        protected readonly DataMemory memory;
        protected readonly ErrorList errors;
        protected readonly IGeneratorFasade generator;

        public Generator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator)
        {
            this.tokenizer = tokenizer;
            this.code = code;
            this.memory = memory;
            this.errors = errors;
            this.generator = generator;
        }

    }
}
