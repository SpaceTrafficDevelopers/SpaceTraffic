using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public class FunctionGenerator : Generator
    {
        public FunctionGenerator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator) 
            : base (tokenizer, code, memory, errors, generator)
        {
        }

        public IOperand Function()
        {
            IOperand result, newResult;

            if (generator.CurrentSymbol == Symbols.IntSym)
            {
                generator.NextSymbol();

                result = generator.Factor();

                if (result.Type == VariableType.Double)
                {
                    IOperand castVar = memory.GenerateNewResult(VariableType.Int);
                    code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, castVar);
                    result = castVar;
                }
                else if (result.Type != VariableType.Int)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.IntWrongParam,
                        "Expecting integer parameter");
                }
            }
            else if (generator.CurrentSymbol == Symbols.LenSSym)
            {
                generator.NextSymbol();

                result = generator.StringFactor(false);

                newResult = memory.GenerateNewResult(VariableType.Int);

                code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "LENS"), null, newResult);
                code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

                result = newResult;
            }
            else if (generator.CurrentSymbol == Symbols.LenSym)
            {
                generator.NextSymbol();

                switch (tokenizer.CurrentSymbol)
                {
                    case Symbols.IntArrayIdent:
                        result = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Int);
                        newResult = memory.GenerateNewArrayResult(VariableType.Int);
                        break;
                    case Symbols.DoubleArrayIdent:
                        result = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Double);
                        newResult = memory.GenerateNewArrayResult(VariableType.Double);
                        break;
                    case Symbols.StringArrayIdent:
                        result = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.String);
                        newResult = memory.GenerateNewArrayResult(VariableType.String);
                        break;
                    default: throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingArrayIdentifier,
                        "Expecting array");
                }

                code.GenInstruction(InstructionCode.ARI, OperationCode.None, result, new Constant(VariableType.Int, -1), null);
                code.GenInstruction(InstructionCode.STO, OperationCode.None, result, null, newResult);

                result = newResult;

                newResult = memory.GenerateNewResult(VariableType.Int);

                code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "LEN"), null, newResult);
                code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

                result = newResult;

                generator.NextSymbol();

                if (generator.CurrentSymbol != Symbols.RHParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen,
                        "Missing right square bracket");
                }

                generator.NextSymbol();
            }
            else if (generator.CurrentSymbol == Symbols.SqrtSym)
            {
                generator.NextSymbol();

                result = generator.Factor();

                if (result.Type == VariableType.Int)
                {
                    IOperand castVar = memory.GenerateNewResult(VariableType.Double);
                    code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, castVar);
                    result = castVar;
                }

                newResult = memory.GenerateNewResult(VariableType.Double);

                code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "SQRT"), null, newResult);
                code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

                result = newResult;
            }
            else if (generator.CurrentSymbol == Symbols.RndSym)
            {
                generator.NextSymbol();

                result = memory.GenerateNewResult(VariableType.Double);

                code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "RND"), null, result);
            }
            else if (generator.CurrentSymbol == Symbols.GetSym)
            {
                generator.NextSymbol();

                switch (generator.CurrentSymbol)
                {
                    case Symbols.PriceSym:
                        generator.NextSymbol();
                        result = GetPrice();
                        break;
                    case Symbols.FuelSym:
                        generator.NextSymbol();
                        result = GetFuel();
                        break;
                    case Symbols.WearSym:
                        generator.NextSymbol();
                        result = GetWear();
                        break;
                    case Symbols.SpaceSym:
                        generator.NextSymbol();
                        result = GetSpace();
                        break;
                    case Symbols.FlyTimeSym:
                        generator.NextSymbol();
                        result = GetFlyTime();
                        break;
                    case Symbols.SupplySym:
                        generator.NextSymbol();
                        result = GetSupply();
                        break;
                    default:
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.UnexpectingFunction,
                            "Required function doesn't supported");
                }
            }
            else
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingFunction,
                    "Expecting function");
            }

            return result;
        }

        private IOperand GetPrice()
        {
            IOperand result, newResult;

            result = memory.GenerateNewResult(VariableType.Double);

            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "GETPRICE"), null, result);

            newResult = generator.StringFactor(false);

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, newResult, null, null);

            if (generator.CurrentSymbol == Symbols.FromSym)
            {
                generator.NextSymbol();

                newResult = generator.StringFactor(false);

                code.GenInstruction(InstructionCode.PAR, OperationCode.None, newResult, null, null);
            }

            return result;
        }

        public IOperand GetFuel()
        {
            IOperand result;

            result = memory.GenerateNewResult(VariableType.Double);

            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "GETFUEL"), null, result);

            return result;
        }

        public IOperand GetWear()
        {
            IOperand result;

            result = memory.GenerateNewResult(VariableType.Int);

            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "GETWEAR"), null, result);

            return result;
        }

        private IOperand GetSpace()
        {
            IOperand result, newResult;

            result = memory.GenerateNewResult(VariableType.Int);

            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "GETSPACE"), null, result);

            newResult = generator.StringFactor(false);

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, newResult, null, null);

            return result;
        }

        private IOperand GetFlyTime()
        {
            IOperand result, newResult;

            result = memory.GenerateNewResult(VariableType.Int);

            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "GETFLYTIME"), null, result);

            newResult = generator.StringFactor(false);

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, newResult, null, null);

            return result;
        }

        private IOperand GetSupply()
        {
            IOperand result, newResult;

            result = memory.GenerateNewResult(VariableType.Int);

            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "GETSUPPLY"), null, result);

            newResult = generator.StringFactor(false);

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, newResult, null, null);

            if (generator.CurrentSymbol == Symbols.FromSym)
            {
                generator.NextSymbol();

                newResult = generator.StringFactor(false);

                code.GenInstruction(InstructionCode.PAR, OperationCode.None, newResult, null, null);
                code.GenInstruction(InstructionCode.PAR, OperationCode.None, null, null, null);
            }
            else if (generator.CurrentSymbol == Symbols.InSym)
            {
                generator.NextSymbol();

                newResult = generator.StringFactor(false);

                code.GenInstruction(InstructionCode.PAR, OperationCode.None, null, null, null);
                code.GenInstruction(InstructionCode.PAR, OperationCode.None, newResult, null, null);
            }

            return result;
        }
    }
}
