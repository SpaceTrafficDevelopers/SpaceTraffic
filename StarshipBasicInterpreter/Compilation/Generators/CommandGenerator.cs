using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;
using StarshipBasicInterpreter.Interpreter;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public class CommandGenerator : Generator
    {
        private LabelJumps labelJumps;

        public CommandGenerator(LexicalAnalyzer tokenizer, Code code, DataMemory memory, ErrorList errors, IGeneratorFasade generator, LabelJumps labelJumps)
            : base(tokenizer, code, memory, errors, generator)
        {
            this.labelJumps = labelJumps;
        }

        public void Command()
        {
            if (generator.CurrentSymbol == Symbols.EndOfLine) // empty command
            {
                generator.NextSymbol();

                return;
            }

            switch (generator.CurrentSymbol)
            {
                case Symbols.LabelIdent:
                    Label();
                    break;
                case Symbols.GoSym:
                    GoTo();
                    break;
                case Symbols.IfSym:
                    If();
                    break;
                case Symbols.PrintSym:
                    Print();
                    break;
                case Symbols.DimSym:
                    Dim();
                    break;
                case Symbols.LetSym:
                    Let();
                    break;
                case Symbols.ForSym:
                    ForCycle();
                    break;
                case Symbols.FlySym:
                    FlyTo();
                    break;
                case Symbols.RepairSym:
                    Repair();
                    break;
                case Symbols.LdCargoSym:
                    LdCargo();
                    break;
                case Symbols.UldCargoSym:
                    UldCargo();
                    break;
                case Symbols.BuySym:
                    Buy();
                    break;
                case Symbols.SellSym:
                    Sell();
                    break;
                default: throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.UnknownCommand,
                    "Used uknown command");
            }
        }

        private void Label()
        {
            LabelJump jump = labelJumps.GetLabelJump(tokenizer.CurrentIdentifier);
            jump.LabelLine = code.AddressCounter;

            generator.NextSymbol();

            if (generator.CurrentSymbol != Symbols.EndOfLine)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                    "Expecting end of line");
            }

            generator.NextSymbol();
        }

        private void GoTo()
        {
            generator.NextSymbol();

            if (generator.CurrentSymbol != Symbols.ToSym)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingTo,
                    "Command TO has to folow after GO command");
            }

            generator.NextSymbol();

            if (generator.CurrentSymbol != Symbols.NoTypeIdent)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingLabelIdentifier,
                    "Expecting label identifier after GO TO");
            }

            LabelJump jump = labelJumps.GetLabelJump(tokenizer.CurrentIdentifier);
            jump.AddProgramLine(tokenizer.CurrentLineNumber);
            jump.AddJumpLine(code.AddressCounter);

            code.GenInstruction(InstructionCode.JMP, OperationCode.None, null, null, null);

            generator.NextSymbol();

            if (generator.CurrentSymbol != Symbols.EndOfLine)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                    "Expecting end of line");
            }

            generator.NextSymbol();
        }

        private void If()
        {
            IOperand result;
            generator.NextSymbol();

            result = generator.Expresion();

            if (result.Type == VariableType.Double)
            {
                IOperand result2 = memory.GenerateNewResult(VariableType.Int);
                code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, result2);
                result = result2;
            }

            if (generator.CurrentSymbol != Symbols.ThenSym)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingThen,
                    "Expecting command THEN");
            }

            int instructionNum = code.AddressCounter;

            code.GenInstruction(InstructionCode.JIF, OperationCode.None, null, result, null);

            generator.NextSymbol();

            if (generator.CurrentSymbol == Symbols.EndOfLine)
            {
                generator.NextSymbol();

                generator.CommadGroup(Symbols.EndIfSym);

                generator.NextSymbol();

                if (generator.CurrentSymbol != Symbols.EndOfLine)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                        "Expecting end of line");
                }

                generator.NextSymbol();
            }
            else
            {
                Command();
            }

            code[instructionNum].LOperand = new Constant(VariableType.Int, code.AddressCounter);
        }

        private void Print()
        {
            IOperand result;
            generator.NextSymbol();

            result = generator.StringExpresion();

            if (generator.CurrentSymbol != Symbols.EndOfLine)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                    "Expecting end of line");
            }

            generator.NextSymbol();

            code.GenInstruction(InstructionCode.WRI, OperationCode.None, result, null, null);
        }

        private void Dim()
        {
            IOperand result;
            generator.NextSymbol();

            IOperand variable = null;

            if (generator.CurrentSymbol == Symbols.IntArrayIdent)
            {
                variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Int);
            }
            else if (generator.CurrentSymbol == Symbols.DoubleArrayIdent)
            {
                variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Double);
            }
            else if (generator.CurrentSymbol == Symbols.StringArrayIdent)
            {
                variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.String);
            }
            else
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingArrayIdentifier,
                    "Command DIM expecting array");
            }

            generator.NextSymbol();

            result = generator.Expresion();

            if (generator.CurrentSymbol != Symbols.RHParen)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen,
                    "Missing right square bracket");
            }

            generator.NextSymbol();

            if (generator.CurrentSymbol != Symbols.EndOfLine)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                    "Expecting end of line");
            }

            generator.NextSymbol();

            code.GenInstruction(InstructionCode.ARA, OperationCode.None, variable, result, null);
        }

        private void Let()
        {
            IOperand result;
            generator.NextSymbol();

            if (generator.CurrentSymbol == Symbols.StringIdent)
            {
                IOperand strVariable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.String);

                generator.NextSymbol();

                if (generator.CurrentSymbol != Symbols.EqlOp)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingBecomes,
                        "Command LET expecting assignment");
                }

                generator.NextSymbol();

                result = generator.StringExpresion();

                if (generator.CurrentSymbol != Symbols.EndOfLine)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                        "Expecting end of line");
                }

                generator.NextSymbol();

                code.GenInstruction(InstructionCode.STO, OperationCode.None, result, null, strVariable);
            }
            else if (generator.CurrentSymbol == Symbols.StringArrayIdent)
            {
                IOperand strVariable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.String);
                IOperand index = new Constant(VariableType.Int, -1);
                bool isStringArray = false;

                generator.NextSymbol();

                if (generator.CurrentSymbol == Symbols.RHParen)
                {
                    isStringArray = true;
                }
                else
                {
                    index = generator.Expresion();

                    if (index.Type == VariableType.Double)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion,
                            "Cannot convert string array to number");
                    }

                    if (generator.CurrentSymbol != Symbols.RHParen)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen,
                            "Missing right square bracket");
                    }
                }

                generator.NextSymbol();

                if (generator.CurrentSymbol != Symbols.EqlOp)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingBecomes,
                        "Command LET expecting assignment");
                }

                generator.NextSymbol();

                if (isStringArray)
                {
                    result = generator.StringArrayFactor();
                }
                else
                {
                    result = generator.StringExpresion();
                }

                if (generator.CurrentSymbol != Symbols.EndOfLine)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                        "Expecting end of line");
                }

                generator.NextSymbol();

                code.GenInstruction(InstructionCode.ARI, OperationCode.None, strVariable, index, null);
                code.GenInstruction(InstructionCode.STO, OperationCode.None, result, null, strVariable);
            }
            else if ((generator.CurrentSymbol == Symbols.IntIdent) || (generator.CurrentSymbol == Symbols.DoubleIdent))
            {
                IOperand variable;

                if (generator.CurrentSymbol == Symbols.IntIdent)
                {
                    variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Int);
                }
                else
                {
                    variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Double);
                }

                generator.NextSymbol();

                if (generator.CurrentSymbol != Symbols.EqlOp)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingBecomes,
                        "Command LET expecting assignment");
                }

                generator.NextSymbol();

                result = generator.Expresion();

                if (generator.CurrentSymbol != Symbols.EndOfLine)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                        "Expecting end of line");
                }

                generator.NextSymbol();

                if (result.Type == VariableType.Double)
                {
                    if (variable.Type == VariableType.Int)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion,
                            "Cannot convert double to integer");
                    }
                }
                else
                {
                    if (variable.Type == VariableType.Double)
                    {
                        IOperand castVar = memory.GenerateNewResult(VariableType.Double);
                        code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, castVar);
                        result = castVar;
                    }
                }

                code.GenInstruction(InstructionCode.STO, OperationCode.None, result, null, variable);
            }
            else if ((generator.CurrentSymbol == Symbols.IntArrayIdent) || (generator.CurrentSymbol == Symbols.DoubleArrayIdent))
            {
                IOperand variable = memory.GetVariable(tokenizer.CurrentIdentifier, (generator.CurrentSymbol == Symbols.IntArrayIdent) ? VariableType.Int : VariableType.Double);
                IOperand index = new Constant(VariableType.Int, -1);
                bool isArray = false;

                generator.NextSymbol();

                if (generator.CurrentSymbol == Symbols.RHParen)
                {
                    isArray = true;
                }
                else
                {
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
                }

                generator.NextSymbol();

                if (generator.CurrentSymbol != Symbols.EqlOp)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingBecomes,
                        "Command LET expecting assignment");
                }

                generator.NextSymbol();

                if (isArray)
                {
                    result = generator.ArrayFactor();
                }
                else
                {
                    result = generator.Expresion();
                }

                if (generator.CurrentSymbol != Symbols.EndOfLine)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                        "Expecting end of line");
                }

                generator.NextSymbol();

                if (isArray && (result.Type !=variable.Type))
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DifferentArrayTypes,
                        "Type of arrays is different");
                }
                else
                {
                    if (result.Type == VariableType.Double)
                    {
                        if (variable.Type == VariableType.Int)
                        {
                            throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion,
                                "Cannot convert double to integer");
                        }
                    }
                    else
                    {
                        if (variable.Type == VariableType.Double)
                        {
                            IOperand castVar = memory.GenerateNewResult(VariableType.Double);
                            code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, castVar);
                            result = castVar;
                        }
                    }
                }

                code.GenInstruction(InstructionCode.ARI, OperationCode.None, variable, index, null);
                code.GenInstruction(InstructionCode.STO, OperationCode.None, result, null, variable);
            }
            else
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingIdentifier,
                    "Expecting identifier");
            }
        }

        private void ForCycle()
        {
            IOperand result, index = null, index2 = null;
            generator.NextSymbol();

            string variableName = tokenizer.CurrentIdentifier;
            bool isArray = false;
            VariableType variableType = VariableType.String;

            switch (generator.CurrentSymbol)
            {
                case Symbols.IntIdent:
                    variableType = VariableType.Int;
                    break;
                case Symbols.IntArrayIdent:
                    variableType = VariableType.Int;
                    isArray = true;
                    break;
                case Symbols.DoubleIdent:
                    variableType = VariableType.Double;
                    break;
                case Symbols.DoubleArrayIdent:
                    variableType = VariableType.Double;
                    isArray = true;
                    break;
                default:
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingIdentifier,
                        "Expecting identifier");
            }
            IOperand variable = memory.GetVariable(variableName, variableType);

            generator.NextSymbol();

            if (isArray)
            {
                index = generator.Expresion();

                if (index.Type == VariableType.Double)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion,
                        "Cannot convert double to integer");
                }

                if (generator.CurrentSymbol != Symbols.RHParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen,
                        "Missing right square bracket");
                }

                generator.NextSymbol();
            }

            if (generator.CurrentSymbol != Symbols.EqlOp)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingBecomes,
                    "Command FOR expecting assignment");
            }

            generator.NextSymbol();

            result = generator.Expresion();

            result = CastIntDouble(result, variableType);

            if (isArray)
            {
                code.GenInstruction(InstructionCode.ARI, OperationCode.None, variable, index, null);
            }
            code.GenInstruction(InstructionCode.STO, OperationCode.None, result, null, variable);

            if (generator.CurrentSymbol != Symbols.ToSym)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingTo,
                    "Missing command TO in FOR loop");
            }

            generator.NextSymbol();

            int conditionInstructionNum = code.AddressCounter;

            result = generator.Expresion();

            IOperand newResult;

            if (isArray)
            {
                code.GenInstruction(InstructionCode.ARI, OperationCode.None, variable, index, null);
            }

            if ((result.Type != VariableType.Int) || (variableType != VariableType.Int))
            {
                if (result.Type == VariableType.Int)
                {
                    newResult = memory.GenerateNewResult(VariableType.Double);
                    code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, newResult);
                    result = newResult;
                }
                if (variableType == VariableType.Int)
                {
                    newResult = memory.GenerateNewResult(VariableType.Double);
                    code.GenInstruction(InstructionCode.CST, OperationCode.None, variable, null, newResult);
                    variable = newResult;
                }
            }

            int tmoInstructionNum = code.AddressCounter;

            code.GenInstruction(InstructionCode.TMO, OperationCode.LEq, new Constant(VariableType.Int, code.AddressCounter + 1), null, null);

            newResult = memory.GenerateNewResult(VariableType.Int);
            code.GenInstruction(InstructionCode.OPR, OperationCode.LEq, variable, result, newResult);

            int jumpInstructionNum = code.AddressCounter;

            code.GenInstruction(InstructionCode.JIF, OperationCode.None, null, newResult, null);

            IOperand step;
            if (variableType == VariableType.Int)
            {
                step = new Constant(variableType, 1);
            }
            else
            {
                step = new Constant(variableType, (double)1);
            }

            if (generator.CurrentSymbol == Symbols.StepSym)
            {
                generator.NextSymbol();

                result = generator.Expresion();

                result = CastIntDouble(result, variableType);

                code[tmoInstructionNum].ROperand = result;

                step = result;
            }

            if (generator.CurrentSymbol != Symbols.EndOfLine)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                    "Expecting end of line");
            }

            generator.NextSymbol();

            generator.CommadGroup(Symbols.NextSym);

            generator.NextSymbol();

            switch (generator.CurrentSymbol)
            {
                case Symbols.IntIdent:
                    variable = memory.GetVariable(variableName, VariableType.Int);
                    break;
                case Symbols.IntArrayIdent:
                    variable = memory.GetVariable(variableName, VariableType.Int);
                    break;
                case Symbols.DoubleIdent:
                    variable = memory.GetVariable(variableName, VariableType.Double);
                    break;
                case Symbols.DoubleArrayIdent:
                    variable = memory.GetVariable(variableName, VariableType.Double);
                    break;
                default:
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingIdentifier,
                        "Expecting identifier");
            }

            if (variable == null)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingIdentifier,
                    "Expecting identifier");
            }

            if (variableName.CompareTo(tokenizer.CurrentIdentifier) != 0)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.NextWrongVal,
                    "Unexpected value");
            }

            generator.NextSymbol();

            if (isArray)
            {
                index2 = generator.Expresion();

                if (index2.Type == VariableType.Double)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion,
                        "Cannot convert array to number");
                }

                if (generator.CurrentSymbol != Symbols.RHParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen,
                        "Missing right square bracket");
                }

                code.GenInstruction(InstructionCode.TER, OperationCode.NEq, index, index2, new Constant(VariableType.Int, (int)RuntimeExceptionCode.DifferentIndex));

                generator.NextSymbol();
            }

            if (generator.CurrentSymbol != Symbols.EndOfLine)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                    "Expecting end of line");
            }

            generator.NextSymbol();

            code.GenInstruction(InstructionCode.OPR, OperationCode.Plus, variable, step, variable);

            code.GenInstruction(InstructionCode.JMP, OperationCode.None, new Constant(VariableType.Int, conditionInstructionNum), null, null);

            code[jumpInstructionNum].LOperand = new Constant(VariableType.Int, code.AddressCounter);
        }

        private void FlyTo()
        {
            generator.NextSymbol();

            if (generator.CurrentSymbol != Symbols.ToSym)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingTo,
                    "Command TO has to folow after FLY command");
            }

            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "FLYTO"), null, null);

            generator.NextSymbol();

            IOperand result;

            while (true)
            {
                result = generator.StringExpresion();

                code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

                if (generator.CurrentSymbol == Symbols.EndOfLine)
                    break;

                if (generator.CurrentSymbol != Symbols.Comma)
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingComma,
                        "Expecting comma separator");

                generator.NextSymbol();
            }
        }

        private void Repair()
        {
            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "REPAIR"), null, null);

            generator.NextSymbol();

            if (generator.CurrentSymbol != Symbols.EndOfLine)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                    "Expecting end of line");
            }

            generator.NextSymbol();
        }

        private void LdCargo()
        {
            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "LDCARGO"), null, null);

            generator.NextSymbol();

            IOperand result;
            result = generator.StringExpresion();

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

            if (generator.CurrentSymbol != Symbols.AmntSym)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingAmnt,
                    "Expecting AMNT command");
            }

            generator.NextSymbol();

            result = generator.Expresion();

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

            if (generator.CurrentSymbol != Symbols.FromSym)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingFrom,
                    "Expecting FROM command");
            }

            generator.NextSymbol();

            result = generator.StringExpresion();

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

            if (generator.CurrentSymbol != Symbols.EndOfLine)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                    "Expecting end of line");
            }

            generator.NextSymbol();
        }

        private void UldCargo()
        {
            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "ULDCARGO"), null, null);

            generator.NextSymbol();

            IOperand result = generator.StringExpresion();

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

            if (generator.CurrentSymbol != Symbols.AmntSym)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingAmnt,
                    "Expecting AMNT command");
            }

            generator.NextSymbol();

            result = generator.Expresion();

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

            if (generator.CurrentSymbol != Symbols.ToSym)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingFrom,
                    "Expecting FROM command");
            }

            generator.NextSymbol();

            result = generator.StringExpresion();

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

            if (generator.CurrentSymbol != Symbols.EndOfLine)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                    "Expecting end of line");
            }

            generator.NextSymbol();
        }

        private void Buy()
        {
            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "BUY"), null, null);

            generator.NextSymbol();

            IOperand result = generator.StringExpresion();

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

            if (generator.CurrentSymbol != Symbols.AmntSym)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingAmnt,
                    "Expecting AMNT command");
            }

            generator.NextSymbol();

            result = generator.Expresion();

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

            if (generator.CurrentSymbol != Symbols.MaxPSym)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingMaxP,
                    "Expecting MAXP command");
            }

            generator.NextSymbol();

            result = generator.Expresion();

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

            if (generator.CurrentSymbol == Symbols.FromSym)
            {
                generator.NextSymbol();

                result = generator.StringExpresion();

                code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);
            }

            if (generator.CurrentSymbol != Symbols.EndOfLine)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                    "Expecting end of line");
            }

            generator.NextSymbol();
        }

        private void Sell()
        {
            code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "SELL"), null, null);

            generator.NextSymbol();

            IOperand result = generator.StringExpresion();

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

            if (generator.CurrentSymbol != Symbols.AmntSym)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingAmnt,
                    "Expecting AMNT command");
            }

            generator.NextSymbol();

            result = generator.Expresion();

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

            if (generator.CurrentSymbol != Symbols.MinPSym)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingMinP,
                    "Expecting MINP command");
            }

            generator.NextSymbol();

            result = generator.Expresion();

            code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

            if (generator.CurrentSymbol == Symbols.ToSym)
            {
                generator.NextSymbol();

                result = generator.StringExpresion();

                code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);
            }

            if (generator.CurrentSymbol != Symbols.EndOfLine)
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL,
                    "Expecting end of line");
            }

            generator.NextSymbol();
        }

        private IOperand CastIntDouble(IOperand result, VariableType variableType)
        {
            if (result.Type == VariableType.Double)
            {
                if (variableType == VariableType.Int)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion,
                        "Cannot convert double to integer");
                }
            }
            else
            {
                if (variableType == VariableType.Double)
                {
                    IOperand castVar = memory.GenerateNewResult(VariableType.Double);
                    code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, castVar);
                    result = castVar;
                }
            }
            return result;
        }
    }
}
