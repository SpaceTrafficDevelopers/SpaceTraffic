using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.ProgramCode;
using StarshipBasicInterpreter.Compilation.Generators;

namespace StarshipBasicInterpreter.Compilation
{
    public class CodeGenerator : IGeneratorFasade
    {
        private LexicalAnalyzer tokenizer;
        private Symbols currentSymbol;
        private Code code;
        private DataMemory memory;
        private LabelJumps labelJumps;
        private ErrorList errors;

        private TermGenerator termGenerator;
        private LogicFactorGenerator logicFactorGenerator;
        private FactorGenerator factorGenerator;
        private ArrayFactorGenerator arrayFactorGenerator;
        private StringArrayFactorGenerator stringArrayFactorGenerator;
        private StringFactorGenerator stringFactorGenerator;
        private StringExpresionGenerator stringExpresionGenerator;
        private FunctionGenerator functionGenerator;
        private ExpresionGenerator expresionGenerator;
        private AndExpresionGenerator andExpresionGenerator;
        private LogicExpresionGenerator logicExpresionGenerator;
        private LogicTermGenerator logicTermGenerator;
        private CommandGenerator commandGenerator;
        private CommadGroupGenerator commadGroupGenerator;

        public CodeGenerator()
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

            arrayFactorGenerator = new ArrayFactorGenerator(tokenizer, code, memory, errors, this);
            factorGenerator = new FactorGenerator(tokenizer, code, memory, errors, this);
            logicFactorGenerator = new LogicFactorGenerator(tokenizer, code, memory, errors, this);
            termGenerator = new TermGenerator(tokenizer, code, memory, errors, this);
            stringExpresionGenerator = new StringExpresionGenerator(tokenizer, code, memory, errors, this);
            stringFactorGenerator = new StringFactorGenerator(tokenizer, code, memory, errors, this);
            stringArrayFactorGenerator = new StringArrayFactorGenerator(tokenizer, code, memory, errors, this);
            functionGenerator = new FunctionGenerator(tokenizer, code, memory, errors, this);
            expresionGenerator = new ExpresionGenerator(tokenizer, code, memory, errors, this);
            andExpresionGenerator = new AndExpresionGenerator(tokenizer, code, memory, errors, this);
            logicExpresionGenerator = new LogicExpresionGenerator(tokenizer, code, memory, errors, this);
            logicTermGenerator = new LogicTermGenerator(tokenizer, code, memory, errors, this);
            commandGenerator = new CommandGenerator(tokenizer, code, memory, errors, this, labelJumps);
            commadGroupGenerator = new CommadGroupGenerator(tokenizer, code, memory, errors, this);


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
                        errors.AddCompilationException(new CompilationException(line, ErrorCode.GoToUnexistingLabel,
                            "Required label doesn't exist"));
                    }
                }
            }
        } 

        public Code Code
        {
            get { return code; }
        }

        public DataMemory Memory
        {
            get { return memory; }
        }

        public ErrorList Errors
        {
            get { return errors; }
        }

        public Symbols CurrentSymbol
        {
            get { return currentSymbol; }
        }

        public void NextSymbol()
        {
            currentSymbol = tokenizer.NextSymbol();
        }

        public void NextLine()
        {
            currentSymbol = tokenizer.NextLine();
        }


        public IOperand Expresion()
        {
            return expresionGenerator.Expresion();
        }

        public IOperand Factor()
        {
            return factorGenerator.Factor();
        }

        public IOperand Function()
        {
            return functionGenerator.Function();
        }

        public IOperand StringExpresion()
        {
            return stringExpresionGenerator.StringExpresion();
        }

        public IOperand StringFactor(bool possibleConversion)
        {
            return stringFactorGenerator.StringFactor(possibleConversion);
        }

        public IOperand LogicFactor()
        {
            return logicFactorGenerator.LogicFactor();
        }

        public IOperand Term()
        {
            return termGenerator.Term();
        }

        public IOperand AndExpresion()
        {
            return andExpresionGenerator.AndExpresion();
        }

        public IOperand LogicExpresion()
        {
            return logicExpresionGenerator.LogicExpresion();
        }

        public IOperand LogicTerm()
        {
            return logicTermGenerator.LogicTerm();
        }

        public void Command()
        {
            commandGenerator.Command();
        }

        public void CommadGroup(Symbols returnSymbol)
        {
            commadGroupGenerator.CommadGroup(returnSymbol);
        }

        public IOperand StringArrayFactor()
        {
            return stringArrayFactorGenerator.StringArrayFactor();
        }

        public IOperand ArrayFactor()
        {
            return arrayFactorGenerator.ArrayFactor();
        }
    }
}
