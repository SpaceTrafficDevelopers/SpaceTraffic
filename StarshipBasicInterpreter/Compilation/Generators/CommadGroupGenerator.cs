using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public class CommadGroupGenerator : Generator
    {
        public CommadGroupGenerator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator)
            : base(tokenizer, code, memory, errors, generator)
        {
        }

        public void CommadGroup(Symbols returnSymbol)
        {
            if (generator.CurrentSymbol == Symbols.EndOfProgram)
            {
                return;
            }
            else if (generator.CurrentSymbol == returnSymbol)
            {
                return;
            }
            else
            {
                try
                {
                    generator.Command();
                }
                catch (CompilationException e)
                {
                    errors.AddCompilationException(e);

                    generator.NextLine();
                }

                CommadGroup(returnSymbol);
            }
        }
    }
}
