using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public class TermGenerator : Generator
    {
        public TermGenerator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator)
            : base(tokenizer, code, memory, errors, generator)
        {
        }

        public IOperand Term()
        {
            IOperand newResult, newResult2, newResult3;

            newResult = generator.LogicFactor();

            while ((generator.CurrentSymbol == Symbols.TimesOp) || (generator.CurrentSymbol == Symbols.DivideOp)
                || (generator.CurrentSymbol == Symbols.DivSym) || (generator.CurrentSymbol == Symbols.ModSym))
            {
                OperationCode opType;

                switch (generator.CurrentSymbol)
                {
                    case Symbols.TimesOp:
                        opType = OperationCode.Times;
                        break;
                    case Symbols.DivideOp:
                        opType = OperationCode.DivD;
                        break;
                    case Symbols.DivSym:
                        opType = OperationCode.DivI;
                        break;
                    default:
                        opType = OperationCode.Mod;
                        break;
                }

                generator.NextSymbol();

                newResult2 = generator.LogicFactor();

                if ((opType == OperationCode.DivI) || (opType == OperationCode.Mod))
                {
                    if ((newResult.Type != VariableType.Int) || (newResult2.Type != VariableType.Int))
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingIntModDiv,
                            "Result of operation isn't integer");
                    }

                    newResult3 = memory.GenerateNewResult(VariableType.Int);
                    code.GenInstruction(InstructionCode.OPR, opType, newResult, newResult2, newResult3);
                }
                else if (opType == OperationCode.DivD)
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

                    newResult3 = memory.GenerateNewResult(VariableType.Double);
                    code.GenInstruction(InstructionCode.OPR, opType, newResult, newResult2, newResult3);
                }
                else
                {
                    if ((newResult.Type == VariableType.Int) && (newResult2.Type == VariableType.Int))
                    {
                        newResult3 = memory.GenerateNewResult(VariableType.Int);
                    }
                    else
                    {
                        newResult3 = memory.GenerateNewResult(VariableType.Double);

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
                }

                newResult = newResult3;
            }

            return newResult;
        }
    }
}
