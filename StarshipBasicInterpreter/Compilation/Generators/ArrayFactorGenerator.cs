using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public class ArrayFactorGenerator : Generator
    {
        public ArrayFactorGenerator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator)
            : base(tokenizer, code, memory, errors, generator)
        {
        }


        public IOperand ArrayFactor()
        {
            IOperand result, newResult;

            if ((generator.CurrentSymbol == Symbols.IntArrayIdent) || (generator.CurrentSymbol == Symbols.DoubleArrayIdent))
            {
                result = memory.GetVariable(tokenizer.CurrentIdentifier, (generator.CurrentSymbol == Symbols.IntArrayIdent) ? VariableType.Int : VariableType.Double);

                generator.NextSymbol();

                if (generator.CurrentSymbol != Symbols.RHParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen, 
                        "Missing right square bracket");
                }

                generator.NextSymbol();

                newResult = memory.GenerateNewArrayResult(result.Type);

                code.GenInstruction(InstructionCode.ARI, OperationCode.None, result, new Constant(VariableType.Int, -1), null);
                code.GenInstruction(InstructionCode.STO, OperationCode.None, result, null, newResult);

                result = newResult;
            }
            else
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingArrayFactor,
                    "Expect array variable");
            }

            return result;
        }
    }
}
