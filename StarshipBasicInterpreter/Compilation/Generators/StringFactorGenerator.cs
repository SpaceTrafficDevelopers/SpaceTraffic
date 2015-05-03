using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public class StringFactorGenerator : Generator
    {
        public StringFactorGenerator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator)
            : base(tokenizer, code, memory, errors, generator)
        {
        }

        public IOperand StringFactor(bool possibleConversion)
        {
            IOperand result;

            if (generator.CurrentSymbol == Symbols.String)
            {
                result = new Constant(VariableType.String, tokenizer.CurrentString);

                generator.NextSymbol();
            }
            else if (generator.CurrentSymbol == Symbols.StringIdent)
            {
                result = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.String);

                generator.NextSymbol();
            }
            else if (generator.CurrentSymbol == Symbols.StringArrayIdent)
            {
                IOperand variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.String);
                IOperand index;

                generator.NextSymbol();

                index = generator.Expresion();

                if (index.Type == VariableType.Double)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion,
                        "Cannot convert string to number");
                }

                if (generator.CurrentSymbol != Symbols.RHParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen,
                        "Missing right square bracket");
                }

                generator.NextSymbol();

                result = memory.GenerateNewResult(VariableType.String);

                code.GenInstruction(InstructionCode.ARI, OperationCode.None, variable, index, null);
                code.GenInstruction(InstructionCode.STO, OperationCode.None, variable, null, result);
            }
            else if (generator.CurrentSymbol == Symbols.LParen)
            {
                generator.NextSymbol();

                if ((generator.CurrentSymbol == Symbols.String) || (generator.CurrentSymbol == Symbols.StringIdent) || (generator.CurrentSymbol == Symbols.LParen))
                {
                    result = generator.StringExpresion();
                }
                else
                {
                    if (possibleConversion)
                    {
                        result = generator.Expresion();

                        IOperand newResult2 = memory.GenerateNewResult(VariableType.String);
                        code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, newResult2);
                        result = newResult2;
                    }
                    else
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingStringFactor,
                            "Expecting string or string variable");
                    }
                }

                if (generator.CurrentSymbol != Symbols.RParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRParen,
                        "Missing right parenthesis");
                }

                generator.NextSymbol();
            }
            else if (possibleConversion)
            {
                result = generator.Term();

                IOperand newResult2 = memory.GenerateNewResult(VariableType.String);
                code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, newResult2);
                result = newResult2;
            }
            else
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingStringFactor,
                    "Expecting string or string variable");
            }

            return result;
        }
    }
}
