using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public class StringExpresionGenerator : Generator
    {
        public StringExpresionGenerator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator)
            : base(tokenizer, code, memory, errors, generator)
        {
        }

        public IOperand StringExpresion()
        {
            IOperand newResult, newResult2, newResult3;

            newResult = generator.StringFactor(false);

            while (generator.CurrentSymbol == Symbols.PlusOp)
            {
                generator.NextSymbol();

                newResult2 = generator.StringFactor(true);

                newResult3 = memory.GenerateNewResult(VariableType.String);
                code.GenInstruction(InstructionCode.OPR, OperationCode.Conc, newResult, newResult2, newResult3);

                newResult = newResult3;
            }

            return newResult;
        }
    }
}
