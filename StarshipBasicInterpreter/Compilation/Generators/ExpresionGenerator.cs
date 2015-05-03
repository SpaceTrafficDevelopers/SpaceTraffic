using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public class ExpresionGenerator : Generator
    {
        public ExpresionGenerator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator)
            : base(tokenizer, code, memory, errors, generator)
        {
        }

        public IOperand Expresion()
        {
            IOperand newResult, newResult2, newResult3;

            newResult = generator.AndExpresion();

            while (generator.CurrentSymbol == Symbols.OrSym)
            {
                generator.NextSymbol();

                newResult2 = generator.AndExpresion();

                if (newResult.Type == VariableType.Double)
                {
                    newResult3 = memory.GenerateNewResult(VariableType.Int);
                    code.GenInstruction(InstructionCode.CST, OperationCode.None, newResult, null, newResult3);
                    newResult = newResult3;
                }
                if (newResult2.Type == VariableType.Double)
                {
                    newResult3 = memory.GenerateNewResult(VariableType.Int);
                    code.GenInstruction(InstructionCode.CST, OperationCode.None, newResult2, null, newResult3);
                    newResult2 = newResult3;
                }

                newResult3 = memory.GenerateNewResult(VariableType.Int);
                code.GenInstruction(InstructionCode.OPR, OperationCode.Or, newResult, newResult2, newResult3);

                newResult = newResult3;
            }

            return newResult;
        }
    }
}
