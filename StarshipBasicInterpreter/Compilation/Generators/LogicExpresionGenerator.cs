using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public class LogicExpresionGenerator : Generator
    {
        public LogicExpresionGenerator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator)
            : base(tokenizer, code, memory, errors, generator)
        {
        }

        public IOperand LogicExpresion()
        {
            IOperand newResult, newResult2, newResult3;

            newResult = generator.LogicTerm();

            while (IsCurrentSymbolComparator())
            {
                OperationCode opType;

                switch (generator.CurrentSymbol)
                {
                    case Symbols.EqlOp:
                        opType = OperationCode.Eql;
                        break;
                    case Symbols.LessOp:
                        opType = OperationCode.Lss;
                        break;
                    case Symbols.LessEqlOp:
                        opType = OperationCode.LEq;
                        break;
                    case Symbols.GtrOp:
                        opType = OperationCode.Gtr;
                        break;
                    case Symbols.GtrEqOp:
                        opType = OperationCode.GEq;
                        break;
                    default:
                        opType = OperationCode.NEq;
                        break;
                }

                generator.NextSymbol();

                newResult2 = generator.LogicTerm();

                if ((newResult.Type != VariableType.Int) || (newResult2.Type != VariableType.Int))
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

                newResult3 = memory.GenerateNewResult(VariableType.Int);
                code.GenInstruction(InstructionCode.OPR, opType, newResult, newResult2, newResult3);

                newResult = newResult3;
            }

            return newResult;
        }

        private bool IsCurrentSymbolComparator()
        {
            return (generator.CurrentSymbol == Symbols.EqlOp) || (generator.CurrentSymbol == Symbols.LessOp) || (generator.CurrentSymbol == Symbols.GtrOp)
                || (generator.CurrentSymbol == Symbols.LessEqlOp) || (generator.CurrentSymbol == Symbols.GtrEqOp) || (generator.CurrentSymbol == Symbols.NotEqlOp);
        } 
    }
}
