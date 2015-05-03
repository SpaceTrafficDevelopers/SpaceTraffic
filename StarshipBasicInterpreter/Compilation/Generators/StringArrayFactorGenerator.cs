using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public class StringArrayFactorGenerator : Generator
    {
        public StringArrayFactorGenerator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator)
            : base(tokenizer, code, memory, errors, generator)
        {
        }

        public IOperand StringArrayFactor()
        {
            IOperand result, newResult;

            if (generator.CurrentSymbol == Symbols.StringArrayIdent)
            {
                result = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.String);

                generator.NextSymbol();

                if (generator.CurrentSymbol != Symbols.RHParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen,
                        "Missing right square bracket");
                }

                generator.NextSymbol();

                newResult = memory.GenerateNewArrayResult(VariableType.String);

                code.GenInstruction(InstructionCode.ARI, OperationCode.None, result, new Constant(VariableType.Int, -1), null);
                code.GenInstruction(InstructionCode.STO, OperationCode.None, result, null, newResult);

                result = newResult;
            }
            else if (generator.CurrentSymbol == Symbols.GetSym)
            {
                generator.NextSymbol();

                return StringArrayGetFunction();
            }
            else
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingStringArrayFactor,
                    "Expecting array of strings");
            }

            return result;
        }

        private IOperand StringArrayGetFunction()
        {
            switch (generator.CurrentSymbol)
            {
                case Symbols.BasesSym:
                    generator.NextSymbol();
                    return GetBases();
                case Symbols.PlanetsSym:
                    generator.NextSymbol();
                    return GetPlanets();
                case Symbols.StarsystemsSym:
                    generator.NextSymbol();
                    return GetStarsystems();
                case Symbols.ExitsSym:
                    generator.NextSymbol();
                    return GetExits();
                default: 
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.UnexpectingFunction,
                        "Required function doesn't supported");
            }
        }

        private IOperand GetBases()
        {
            IOperand result, newResult;

            result = memory.GenerateNewArrayResult(VariableType.String);

            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "GETBASES"), null, result);

            if (generator.CurrentSymbol == Symbols.InSym)
            {
                generator.NextSymbol();

                newResult = generator.StringFactor(false);

                code.GenInstruction(InstructionCode.PAR, OperationCode.None, newResult, null, null);
            }

            return result;
        }

        private IOperand GetPlanets()
        {
            IOperand result, newResult;

            result = memory.GenerateNewArrayResult(VariableType.String);

            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "GETPLANETS"), null, result);

            if (generator.CurrentSymbol == Symbols.InSym)
            {
                generator.NextSymbol();

                newResult = generator.StringFactor(false);

                code.GenInstruction(InstructionCode.PAR, OperationCode.None, newResult, null, null);
            }

            return result;
        }

        private IOperand GetStarsystems()
        {
            IOperand result;

            result = memory.GenerateNewArrayResult(VariableType.String);

            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "GETSTARSYSTEMS"), null, result);

            return result;
        }

        private IOperand GetExits()
        {
            IOperand result;

            result = memory.GenerateNewArrayResult(VariableType.String);

            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "GETEXITS"), null, result);

            return result;
        }
    }
}
