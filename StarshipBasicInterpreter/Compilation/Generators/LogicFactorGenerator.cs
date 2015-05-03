using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public class LogicFactorGenerator : Generator
    {
        public LogicFactorGenerator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator)
            : base(tokenizer, code, memory, errors, generator)
        {
        }

        public IOperand LogicFactor()
        {
            IOperand result, newResult;

            if (generator.CurrentSymbol == Symbols.NotSym)
            {
                generator.NextSymbol();

                newResult = generator.Factor();

                if (newResult.Type == VariableType.Double)
                {
                    IOperand newResult2 = memory.GenerateNewResult(VariableType.Int);
                    code.GenInstruction(InstructionCode.CST, OperationCode.None, newResult, null, newResult2);
                    newResult = newResult2;
                }

                result = memory.GenerateNewResult(VariableType.Int);
                code.GenInstruction(InstructionCode.OPR, OperationCode.Not, newResult, null, result);
            }
            else
            {
                result = generator.Factor();
            }

            return result;
        }
    }
}
