using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public class FactorGenerator : Generator
    {
        public FactorGenerator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator)
            : base(tokenizer, code, memory, errors, generator)
        {
        }

        public IOperand Factor()
        {
            IOperand result, newResult;

            if (generator.CurrentSymbol == Symbols.IntNumber)
            {
                result = new Constant(VariableType.Int, tokenizer.CurrentIntNumber);

                generator.NextSymbol();
            }
            else if (generator.CurrentSymbol == Symbols.DoubleNumber)
            {
                result = new Constant(VariableType.Double, tokenizer.CurrentDoubleValue);

                generator.NextSymbol();
            }
            else if (generator.CurrentSymbol == Symbols.MinusOp)
            {
                generator.NextSymbol();

                newResult = Factor();

                result = memory.GenerateNewResult(newResult.Type);

                code.GenInstruction(InstructionCode.OPR, OperationCode.UMinus, newResult, null, result);
            }
            else if (generator.CurrentSymbol == Symbols.IntIdent)
            {
                result = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Int);

                generator.NextSymbol();
            }
            else if (generator.CurrentSymbol == Symbols.DoubleIdent)
            {
                result = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Double);

                generator.NextSymbol();
            }
            else if (generator.CurrentSymbol == Symbols.IntArrayIdent)
            {
                IOperand variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Int);
                IOperand index;

                generator.NextSymbol();

                index = generator.Expresion();

                if (index.Type == VariableType.Double)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion,
                        "Cannot convert array to number");
                }

                if (generator.CurrentSymbol != Symbols.RHParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen,
                        "Missing right square bracket");
                }

                generator.NextSymbol();

                result = memory.GenerateNewResult(VariableType.Int);

                code.GenInstruction(InstructionCode.ARI, OperationCode.None, variable, index, null);
                code.GenInstruction(InstructionCode.STO, OperationCode.None, variable, null, result);
            }
            else if (generator.CurrentSymbol == Symbols.DoubleArrayIdent)
            {
                IOperand variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Double);
                IOperand index;

                generator.NextSymbol();

                index = generator.Expresion();

                if (index.Type == VariableType.Double)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion,
                        "Cannot convert array to number");
                }

                if (generator.CurrentSymbol != Symbols.RHParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen,
                        "Missing right square bracket");
                }

                generator.NextSymbol();

                result = memory.GenerateNewResult(VariableType.Double);

                code.GenInstruction(InstructionCode.ARI, OperationCode.None, variable, index, null);
                code.GenInstruction(InstructionCode.STO, OperationCode.None, variable, null, result);
            }
            else if (generator.CurrentSymbol == Symbols.LParen)
            {
                generator.NextSymbol();

                result = generator.Expresion();

                if (generator.CurrentSymbol != Symbols.RParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRParen,
                        "Missing right parenthesis");
                }

                generator.NextSymbol();
            }
            else
            {
                result = generator.Function();
            }

            return result;
        }
    }
}
