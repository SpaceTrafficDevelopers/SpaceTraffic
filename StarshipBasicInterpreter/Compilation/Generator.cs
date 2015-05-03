using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;

namespace StarshipBasicInterpreter.Compilation
{
    public class Generator
    {
        private LexicalAnalyzer tokenizer;
        private Symbols currentSymbol;
        private Code code;
        private DataMemory memory;
        private LabelJumps labelJumps;
        private ErrorList errors;

        public Generator()
        {
            tokenizer = new LexicalAnalyzer();
        }

        public void Generate(string programCode)
        {
            tokenizer.SetProgramCode(programCode + "\n");

            code = new Code();
            memory = new DataMemory();

            errors = new ErrorList();

            labelJumps = new LabelJumps();

            NextSymbol();

            CommadGroup(Symbols.EndOfProgram);

            for (int i = 0; i < labelJumps.Count; i++)
            {
                if (labelJumps[i].LabelLine != -1)
                {
                    int[] lines = labelJumps[i].CodeLines;
                    foreach (int line in lines)
                    {
                        code[line].LOperand = new Constant(VariableType.Int, labelJumps[i].LabelLine);
                    }
                }
                else
                {
                    int[] lines = labelJumps[i].ProgramLines;
                    foreach (int line in lines)
                    {
                        errors.AddCompilationException(new CompilationException(line, ErrorCode.GoToUnexistingLabel));
                    }
                }
            }
        }

        public Code Code
        {
            get { return code; }
        }

        public ErrorList Errors
        {
            get { return errors; }
        }

        private void CommadGroup(Symbols returnSymbol)
        {
            if (currentSymbol == Symbols.EndOfProgram)
            {
                return;
            }
            else if (currentSymbol == returnSymbol)
            {
                return;
            }
            else
            {
                try
                {
                    Command();
                }
                catch (CompilationException e)
                {
                    errors.AddCompilationException(e);

                    NextLine();
                }

                CommadGroup(returnSymbol);
            }
        }

        private void Command()
        {
            IOperand result;

            if (currentSymbol == Symbols.EndOfLine) // empty command
            {
                NextSymbol();

                return;
            }
            else if (currentSymbol == Symbols.LabelIdent)
            {
                LabelJump jump = labelJumps.GetLabelJump(tokenizer.CurrentIdentifier);
                jump.LabelLine = code.AddressCounter;

                NextSymbol();

                if (currentSymbol != Symbols.EndOfLine)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL);
                }

                NextSymbol();
            }
            else if (currentSymbol == Symbols.GoSym)
            {
                NextSymbol();

                if (currentSymbol != Symbols.ToSym)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingTo);
                }

                NextSymbol();

                if (currentSymbol != Symbols.NoTypeIdent)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingLabelIdentifier);
                }

                LabelJump jump = labelJumps.GetLabelJump(tokenizer.CurrentIdentifier);
                jump.AddProgramLine(tokenizer.CurrentLineNumber);
                jump.AddJumpLine(code.AddressCounter);

                code.GenInstruction(InstructionCode.JMP, OperationCode.None, null, null, null);

                NextSymbol();

                if (currentSymbol != Symbols.EndOfLine)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL);
                }

                NextSymbol();
            }
            else if (currentSymbol == Symbols.IfSym)
            {
                NextSymbol();

                try
                {
                    result = Expresion();

                    if (result.Type == VariableType.Double)
                    {
                        IOperand result2 = memory.GenerateNewResult(VariableType.Int);
                        code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, result2);
                        result = result2;
                    }
                }
                catch
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingExpresion);
                }

                if (currentSymbol != Symbols.ThenSym)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingThen);
                }

                int instructionNum = code.AddressCounter;

                code.GenInstruction(InstructionCode.JIF, OperationCode.None, null, result, null);

                NextSymbol();

                if (currentSymbol == Symbols.EndOfLine)
                {
                    NextSymbol();

                    CommadGroup(Symbols.EndIfSym);

                    NextSymbol();

                    if (currentSymbol != Symbols.EndOfLine)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL);
                    }

                    NextSymbol();
                }
                else
                {
                    Command();
                }

                code[instructionNum].LOperand = new Constant(VariableType.Int, code.AddressCounter);
            }
            else if (currentSymbol == Symbols.PrintSym)
            {
                NextSymbol();

                try
                {
                    result = StringExpresion();
                }
                catch
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingStringExpresion);
                }

                if (currentSymbol != Symbols.EndOfLine)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL);
                }

                NextSymbol();

                code.GenInstruction(InstructionCode.WRI, OperationCode.None, result, null, null);
            }
            else if (currentSymbol == Symbols.DimSym)
            {
                NextSymbol();

                IOperand variable = null;

                if (currentSymbol == Symbols.IntArrayIdent)
                {
                    variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Int);
                }
                else if (currentSymbol == Symbols.DoubleArrayIdent)
                {
                    variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Double);
                }
                else if (currentSymbol == Symbols.StringArrayIdent)
                {
                    variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.String);
                }
                else
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingArrayIdentifier);
                }

                NextSymbol();

                try
                {
                    result = Expresion();
                }
                catch
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingExpresion);
                }

                if (currentSymbol != Symbols.RHParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen);
                }

                NextSymbol();

                if (currentSymbol != Symbols.EndOfLine)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL);
                }

                NextSymbol();

                code.GenInstruction(InstructionCode.ARA, OperationCode.None, variable, result, null);
            }
            else if (currentSymbol == Symbols.LetSym)
            {
                NextSymbol();

                if (currentSymbol == Symbols.StringIdent)
                {
                    IOperand strVariable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.String);

                    NextSymbol();

                    if (currentSymbol != Symbols.EqlOp)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingBecomes);
                    }

                    NextSymbol();

                    try
                    {
                        result = StringExpresion();
                    }
                    catch
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingStringExpresion);
                    }

                    if (currentSymbol != Symbols.EndOfLine)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL);
                    }

                    NextSymbol();

                    code.GenInstruction(InstructionCode.STO, OperationCode.None, result, null, strVariable);
                }
                else if (currentSymbol == Symbols.StringArrayIdent)
                {
                    IOperand strVariable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.String);
                    IOperand index;

                    NextSymbol();

                    try
                    {
                        index = Expresion();
                    }
                    catch
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingExpresion);
                    }

                    if (index.Type == VariableType.Double)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion);
                    }

                    if (currentSymbol != Symbols.RHParen)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen);
                    }

                    NextSymbol();

                    if (currentSymbol != Symbols.EqlOp)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingBecomes);
                    }

                    NextSymbol();

                    try
                    {
                        result = StringExpresion();
                    }
                    catch
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingStringExpresion);
                    }

                    if (currentSymbol != Symbols.EndOfLine)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL);
                    }

                    NextSymbol();

                    code.GenInstruction(InstructionCode.ARI, OperationCode.None, strVariable, index, null);
                    code.GenInstruction(InstructionCode.STO, OperationCode.None, result, null, strVariable);
                }
                else if ((currentSymbol == Symbols.IntIdent) || (currentSymbol == Symbols.DoubleIdent))
                {
                    IOperand variable;

                    if (currentSymbol == Symbols.IntIdent)
                    {
                        variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Int);
                    }
                    else
                    {
                        variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Double);
                    }

                    NextSymbol();

                    if (currentSymbol != Symbols.EqlOp)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingBecomes);
                    }

                    NextSymbol();

                    try
                    {
                        result = Expresion();
                    }
                    catch
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingExpresion);
                    }

                    if (currentSymbol != Symbols.EndOfLine)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL);
                    }

                    NextSymbol();

                    if (result.Type == VariableType.Double)
                    {
                        if (variable.Type == VariableType.Int)
                        {
                            throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion);
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
                else if ((currentSymbol == Symbols.IntArrayIdent) || (currentSymbol == Symbols.DoubleArrayIdent))
                {
                    IOperand variable;

                    if (currentSymbol == Symbols.IntArrayIdent)
                    {
                        variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Int);
                    }
                    else
                    {
                        variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Double);
                    }

                    IOperand index;

                    NextSymbol();

                    try
                    {
                        index = Expresion();
                    }
                    catch
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingExpresion);
                    }

                    if (index.Type == VariableType.Double)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion);
                    }

                    NextSymbol();

                    if (currentSymbol != Symbols.EqlOp)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingBecomes);
                    }

                    NextSymbol();

                    try
                    {
                        result = Expresion();
                    }
                    catch
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingExpresion);
                    }

                    if (currentSymbol != Symbols.EndOfLine)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL);
                    }

                    NextSymbol();

                    if (result.Type == VariableType.Double)
                    {
                        if (variable.Type == VariableType.Int)
                        {
                            throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion);
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

                    code.GenInstruction(InstructionCode.ARI, OperationCode.None, variable, index, null);
                    code.GenInstruction(InstructionCode.STO, OperationCode.None, result, null, variable);
                }
                else
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingIdentifier);
                }
            }
            else if (currentSymbol == Symbols.ForSym)
            {
                NextSymbol();

                IOperand variable = null;
                string variableName = tokenizer.CurrentIdentifier;
                VariableType variableType = VariableType.String;

                if (currentSymbol == Symbols.IntIdent)
                    variableType = VariableType.Int;

                if (currentSymbol == Symbols.DoubleIdent)
                    variableType = VariableType.Double;

                if (variableType == VariableType.String)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingIdentifier);
                }

                variable = memory.GetVariable(variableName, VariableType.Int);

                NextSymbol();

                if (currentSymbol != Symbols.EqlOp)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingBecomes);
                }

                NextSymbol();

                result = Expresion();

                if (result.Type == VariableType.Double)
                {
                    if (variable.Type == VariableType.Int)
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion);
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

                if (currentSymbol != Symbols.ToSym)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingTo);
                }

                NextSymbol();

                int conditionInstructionNum = code.AddressCounter;

                result = Expresion();

                IOperand newResult;

                if ((result.Type != VariableType.Int) || (variable.Type != VariableType.Int))
                {
                    if (result.Type == VariableType.Int)
                    {
                        newResult = memory.GenerateNewResult(VariableType.Double);
                        code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, newResult);
                        result = newResult;
                    }
                    if (variable.Type == VariableType.Int)
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

                if (currentSymbol == Symbols.StepSym)
                {
                    NextSymbol();

                    result = Expresion();

                    if (result.Type == VariableType.Double)
                    {
                        if (variableType == VariableType.Int)
                        {
                            throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion);
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

                    code[tmoInstructionNum].ROperand = result;

                    step = result;
                }

                if (currentSymbol != Symbols.EndOfLine)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL);
                }

                NextSymbol();

                CommadGroup(Symbols.NextSym);

                NextSymbol();

                variable = null;

                if (currentSymbol == Symbols.IntIdent)
                {
                    variable = memory.GetVariable(variableName, VariableType.Int);
                }

                if (currentSymbol == Symbols.DoubleIdent)
                {
                    variable = memory.GetVariable(variableName, VariableType.Double);
                }

                if (variable == null)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingIdentifier);
                }

                if (variableName.CompareTo(tokenizer.CurrentIdentifier) != 0)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.NextWrongVal);
                }

                NextSymbol();

                if (currentSymbol != Symbols.EndOfLine)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingEOL);
                }

                NextSymbol();

                code.GenInstruction(InstructionCode.OPR, OperationCode.Plus, variable, step, variable);

                code.GenInstruction(InstructionCode.JMP, OperationCode.None, new Constant(VariableType.Int, conditionInstructionNum), null, null);

                code[jumpInstructionNum].LOperand = new Constant(VariableType.Int, code.AddressCounter);
            }
        }

        private IOperand Function()
        {
            IOperand result, newResult;

            if (currentSymbol == Symbols.IntSym)
            {
                NextSymbol();

                try
                {
                    result = Factor();
                }
                catch
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.IntWrongParam);
                }

                if (result.Type == VariableType.Double)
                {
                    IOperand castVar = memory.GenerateNewResult(VariableType.Int);
                    code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, castVar);
                    result = castVar;
                }
                else if (result.Type != VariableType.Int)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.IntWrongParam);
                }
            }
            else if (currentSymbol == Symbols.LenSym)
            {
                NextSymbol();

                try
                {
                    result = StringFactor(false);
                }
                catch
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingStringFactor);
                }

                newResult = memory.GenerateNewResult(VariableType.Int);

                code.GenInstruction(InstructionCode.CAL, OperationCode.None, new Constant(VariableType.String, "LEN"), null, newResult);
                code.GenInstruction(InstructionCode.PAR, OperationCode.None, result, null, null);

                result = newResult;
            }
            else if (currentSymbol == Symbols.SqrtSym)
            {
                NextSymbol();

                try
                {
                    result = Factor();
                }
                catch
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingFactor);
                }

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
            else
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingFunction);
            }

            return result;
        }

        private IOperand StringExpresion()
        {
            IOperand newResult, newResult2, newResult3;

            newResult = StringFactor(false);

            while (currentSymbol == Symbols.PlusOp)
            {
                NextSymbol();

                try
                {
                    newResult2 = StringFactor(true);
                }
                catch
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingStringFactor);
                }

                newResult3 = memory.GenerateNewResult(VariableType.String);
                code.GenInstruction(InstructionCode.OPR, OperationCode.Conc, newResult, newResult2, newResult3);

                newResult = newResult3;
            }

            return newResult;
        }

        private IOperand StringFactor(bool possibleConversion)
        {
            IOperand result;

            if (currentSymbol == Symbols.String)
            {
                result = new Constant(VariableType.String, tokenizer.CurrentString);

                NextSymbol();
            }
            else if (currentSymbol == Symbols.StringIdent)
            {
                result = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.String);

                NextSymbol();
            }
            else if (currentSymbol == Symbols.StringArrayIdent)
            {
                IOperand variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.String);
                IOperand index;

                NextSymbol();

                try
                {
                    index = Expresion();
                }
                catch
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingExpresion);
                }

                if (index.Type == VariableType.Double)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion);
                }

                if (currentSymbol != Symbols.RHParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen);
                }

                NextSymbol();

                result = memory.GenerateNewResult(VariableType.String);

                code.GenInstruction(InstructionCode.ARI, OperationCode.None, variable, index, null);
                code.GenInstruction(InstructionCode.STO, OperationCode.None, variable, null, result);
            }
            else if (currentSymbol == Symbols.LParen)
            {
                NextSymbol();

                if ((currentSymbol == Symbols.String) || (currentSymbol == Symbols.StringIdent) || (currentSymbol == Symbols.LParen))
                {
                    result = StringExpresion();
                }
                else
                {
                    if (possibleConversion)
                    {
                        try
                        {
                            result = Expresion();

                            IOperand newResult2 = memory.GenerateNewResult(VariableType.String);
                            code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, newResult2);
                            result = newResult2;
                        }
                        catch
                        {
                            throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingStringFactor);
                        }
                    }
                    else
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingStringFactor);
                    }
                }

                if (currentSymbol != Symbols.RParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRParen);
                }

                NextSymbol();
            }
            else if (possibleConversion)
            {
                try
                {
                    result = Term();

                    IOperand newResult2 = memory.GenerateNewResult(VariableType.String);
                    code.GenInstruction(InstructionCode.CST, OperationCode.None, result, null, newResult2);
                    result = newResult2;
                }
                catch
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingStringFactor);
                }
            }
            else
            {
                throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingStringFactor);
            }

            return result;
        }

        private IOperand Expresion()
        {
            IOperand newResult, newResult2, newResult3;

            newResult = AndExpresion();

            while (currentSymbol == Symbols.OrSym)
            {
                NextSymbol();

                newResult2 = AndExpresion();

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

        private IOperand AndExpresion()
        {
            IOperand newResult, newResult2, newResult3;

            newResult = LogicExpresion();

            while (currentSymbol == Symbols.AndSym)
            {
                NextSymbol();

                newResult2 = LogicExpresion();

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
                code.GenInstruction(InstructionCode.OPR, OperationCode.And, newResult, newResult2, newResult3);

                newResult = newResult3;
            }

            return newResult;
        }

        private IOperand LogicExpresion()
        {
            IOperand newResult, newResult2, newResult3;

            newResult = LogicTerm();

            while (IsCurrentSymbolComparator())
            {
                OperationCode opType;

                switch (currentSymbol)
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


                NextSymbol();

                newResult2 = LogicTerm();

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

        private IOperand LogicTerm()
        {
            IOperand newResult, newResult2, newResult3;

            newResult = Term();

            while ((currentSymbol == Symbols.PlusOp) || (currentSymbol == Symbols.MinusOp))
            {
                OperationCode opType = (currentSymbol == Symbols.PlusOp) ? OperationCode.Plus : OperationCode.Minus;

                NextSymbol();

                newResult2 = Term();

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

        private IOperand Term()
        {
            IOperand newResult, newResult2, newResult3;

            newResult = LogicFactor();

            while ((currentSymbol == Symbols.TimesOp) || (currentSymbol == Symbols.DivideOp)
                || (currentSymbol == Symbols.DivSym) || (currentSymbol == Symbols.ModSym))
            {
                OperationCode opType;

                switch (currentSymbol)
                {
                    case Symbols.TimesOp:
                        opType = OperationCode.Times;
                        break;
                    case Symbols.DivideOp:
                        opType = OperationCode.DivD;
                        break;
                    case Symbols.DimSym:
                        opType = OperationCode.DivI;
                        break;
                    default:
                        opType = OperationCode.Mod;
                        break;
                }

                NextSymbol();

                newResult2 = LogicFactor();

                if ((opType == OperationCode.DivI) || (opType == OperationCode.Mod))
                {
                    if ((newResult.Type != VariableType.Int) || (newResult2.Type != VariableType.Int))
                    {
                        throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingIntModDiv);
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

        private IOperand LogicFactor()
        {
            IOperand result, newResult;

            if (currentSymbol == Symbols.NotSym)
            {
                NextSymbol();

                newResult = Factor();

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
                result = Factor();
            }

            return result;
        }

        private IOperand Factor()
        {
            IOperand result, newResult;

            if (currentSymbol == Symbols.IntNumber)
            {
                result = new Constant(VariableType.Int, tokenizer.CurrentIntNumber);

                NextSymbol();
            }
            else if (currentSymbol == Symbols.DoubleNumber)
            {
                result = new Constant(VariableType.Double, tokenizer.CurrentDoubleValue);

                NextSymbol();
            }
            else if (currentSymbol == Symbols.MinusOp)
            {
                NextSymbol();

                newResult = Factor();

                result = memory.GenerateNewResult(newResult.Type);

                code.GenInstruction(InstructionCode.OPR, OperationCode.UMinus, newResult, null, result);
            }
            else if (currentSymbol == Symbols.IntIdent)
            {
                result = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Int);

                NextSymbol();
            }
            else if (currentSymbol == Symbols.DoubleIdent)
            {
                result = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Double);

                NextSymbol();
            }
            else if (currentSymbol == Symbols.IntArrayIdent)
            {
                IOperand variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Int);
                IOperand index;

                NextSymbol();

                try
                {
                    index = Expresion();
                }
                catch
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingExpresion);
                }

                if (index.Type == VariableType.Double)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion);
                }

                if (currentSymbol != Symbols.RHParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen);
                }

                NextSymbol();

                result = memory.GenerateNewResult(VariableType.Int);

                code.GenInstruction(InstructionCode.ARI, OperationCode.None, variable, index, null);
                code.GenInstruction(InstructionCode.STO, OperationCode.None, variable, null, result);
            }
            else if (currentSymbol == Symbols.DoubleArrayIdent)
            {
                IOperand variable = memory.GetVariable(tokenizer.CurrentIdentifier, VariableType.Double);
                IOperand index;

                NextSymbol();

                try
                {
                    index = Expresion();
                }
                catch
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingExpresion);
                }

                if (index.Type == VariableType.Double)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.DoubleIntConversion);
                }

                if (currentSymbol != Symbols.RHParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRHParen);
                }

                NextSymbol();

                result = memory.GenerateNewResult(VariableType.Double);

                code.GenInstruction(InstructionCode.ARI, OperationCode.None, variable, index, null);
                code.GenInstruction(InstructionCode.STO, OperationCode.None, variable, null, result);
            }
            else if (currentSymbol == Symbols.LParen)
            {
                NextSymbol();

                result = Expresion();

                if (currentSymbol != Symbols.RParen)
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingRParen);
                }

                NextSymbol();
            }
            else
            {
                try
                {
                    result = Function();
                }
                catch
                {
                    throw new CompilationException(tokenizer.CurrentLineNumber, ErrorCode.ExpectingFactor);
                }
            }

            return result;
        }

        private void NextSymbol()
        {
            currentSymbol = tokenizer.NextSymbol();
        }

        private void NextLine()
        {
            currentSymbol = tokenizer.NextLine();
        }

        private bool IsCurrentSymbolComparator()
        {
            return (currentSymbol == Symbols.EqlOp) || (currentSymbol == Symbols.LessOp) || (currentSymbol == Symbols.GtrOp)
                || (currentSymbol == Symbols.LessEqlOp) || (currentSymbol == Symbols.GtrEqOp) || (currentSymbol == Symbols.NotEqlOp);
        }
    }
}
