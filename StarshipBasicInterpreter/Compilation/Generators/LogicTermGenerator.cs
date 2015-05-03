using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public class LogicTermGenerator : Generator
    {
        public LogicTermGenerator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator)
            : base(tokenizer, code, memory, errors, generator)
        {
        }

        public IOperand LogicTerm()
        {
            IOperand newResult, newResult2, newResult3;

            newResult = generator.Term();

            while ((generator.CurrentSymbol == Symbols.PlusOp) || (generator.CurrentSymbol == Symbols.MinusOp))
            {
                OperationCode opType = (generator.CurrentSymbol == Symbols.PlusOp) ? OperationCode.Plus : OperationCode.Minus;

                generator.NextSymbol();

                newResult2 = generator.Term();

                VariableType type = ((newResult.Type == VariableType.Int) && (newResult2.Type == VariableType.Int))
                    ? VariableType.Int : VariableType.Double;

                newResult3 = memory.GenerateNewResult(type);

                if (type == VariableType.Double)
                {
                    IOperand newResult4;
                    if (newResult.Type == VariableType.Int)
                    {
                        newResult4 = memory.GenerateNewResult(VariableType.Double);
                        code.GenInstruction(InstructionCode.CST, OperationCode.None, newResult, null, newResult4);
                        newResult = newResult4;
                    }
                    if (newResult2.Type == VariableType.Int)
                    {
                        newResult4 = memory.GenerateNewResult(VariableType.Double);
                        code.GenInstruction(InstructionCode.CST, OperationCode.None, newResult2, null, newResult4);
                        newResult2 = newResult4;
                    }
                }

                code.GenInstruction(InstructionCode.OPR, opType, newResult, newResult2, newResult3);

                newResult = newResult3;
            }

            return newResult;
        }
    }
}
