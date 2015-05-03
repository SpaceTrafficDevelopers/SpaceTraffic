using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.ProgramCode;
using StarshipBasicInterpreter.Memory;

namespace StarshipBasicInterpreter.Interpreter
{
    public class Interpreter
    {
        private Code code;
        private int pc;

        public Interpreter(Code code)
        {
            this.code = code;
        }

        public void Interpret()
        {
            pc = 0;

            while ((pc < code.Count) && (pc >= 0))
            {
                Instruction instruction = code[pc];
                pc++;

                switch (instruction.InstructionCode)
                {
                    case InstructionCode.STP:
                        pc = code.Count;
                        continue;

                    case InstructionCode.JIF:
                        if ((int)instruction.ROperand.Value == 0)
                            pc = (int)instruction.LOperand.Value;
                        continue;

                    case InstructionCode.JMP:
                        pc = (int)instruction.LOperand.Value;
                        continue;

                    case InstructionCode.WRI:
                        Console.WriteLine(instruction.LOperand.Value.ToString());
                        break;

                    case InstructionCode.STO:
                        instruction.Result.Value = instruction.LOperand.Value;
                        break;

                    case InstructionCode.TMO:
                        if (instruction.ROperand == null)
                            break;

                        OperationCode reverted;
                        switch (instruction.OperationCode)
                        {
                            case OperationCode.LEq:
                                reverted = OperationCode.GEq;
                                break;
                            case OperationCode.GEq:
                                reverted = OperationCode.LEq;
                                break;
                            default:
                                reverted = OperationCode.Eql;
                                break;
                        }
                        // TODO all opp

                        if (instruction.ROperand.Type == VariableType.Int)
                        {
                            if ((int)instruction.ROperand.Value < 0)
                            {
                                code[(int)instruction.LOperand.Value].OperationCode = reverted;
                            }
                            else
                            {
                                code[(int)instruction.LOperand.Value].OperationCode = instruction.OperationCode;
                            }
                        }
                        if (instruction.ROperand.Type == VariableType.Double)
                        {
                            if ((double)instruction.ROperand.Value < 0)
                            {
                                code[(int)instruction.LOperand.Value].OperationCode = reverted;
                            }
                            else
                            {
                                code[(int)instruction.LOperand.Value].OperationCode = instruction.OperationCode;
                            }
                        }
                        break;

                    case InstructionCode.CST:
                        switch (instruction.Result.Type)
                        {
                            case VariableType.Int:
                                if (instruction.LOperand.Type == VariableType.Double)
                                {
                                    instruction.Result.Value = (int)(double)instruction.LOperand.Value;
                                }
                                else
                                {
                                    instruction.Result.Value = instruction.LOperand.Value;
                                }
                                break;

                            case VariableType.Double:
                                if (instruction.LOperand.Type == VariableType.Int)
                                {
                                    instruction.Result.Value = (double)(int)instruction.LOperand.Value;
                                }
                                else
                                {
                                    instruction.Result.Value = instruction.LOperand.Value;
                                }
                                break;

                            default:
                                instruction.Result.Value = instruction.LOperand.Value.ToString();
                                break;
                        }
                        break;

                    case InstructionCode.OPR:
                        switch (instruction.OperationCode)
                        {
                            case OperationCode.Conc:
                                instruction.Result.Value = instruction.LOperand.Value.ToString() + instruction.ROperand.Value.ToString();
                                break;

                            case OperationCode.And:
                                bool left = false;
                                bool right = false;
                                if ((int)instruction.LOperand.Value != 0)
                                {
                                    left = true;
                                }
                                if ((int)instruction.ROperand.Value != 0)
                                {
                                    right = true;
                                }

                                instruction.Result.Value = left && right ? 1 : 0;
                                break;

                            case OperationCode.Or:
                                left = false;
                                right = false;
                                if ((int)instruction.LOperand.Value != 0)
                                {
                                    left = true;
                                }
                                if ((int)instruction.ROperand.Value != 0)
                                {
                                    right = true;
                                }

                                instruction.Result.Value = left || right ? 1 : 0;
                                break;

                            case OperationCode.Not:
                                left = false;
                                if ((int)instruction.LOperand.Value != 0)
                                {
                                    left = true;
                                }

                                instruction.Result.Value = left ? 0 : 1;
                                break;

                            case OperationCode.Eql:
                                bool result = false;
                                if ((instruction.LOperand.Type == VariableType.Int) && (instruction.ROperand.Type == VariableType.Int))
                                {
                                    if ((int)instruction.LOperand.Value == (int)instruction.ROperand.Value)
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    if (Math.Abs((double)instruction.LOperand.Value - (double)instruction.ROperand.Value) < 0.00000001)
                                        result = true;
                                }

                                instruction.Result.Value = result ? 1 : 0;
                                break;

                            case OperationCode.Lss:
                                result = false;
                                if ((instruction.LOperand.Type == VariableType.Int) && (instruction.ROperand.Type == VariableType.Int))
                                {
                                    if ((int)instruction.LOperand.Value < (int)instruction.ROperand.Value)
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    if (((double)instruction.ROperand.Value - (double)instruction.LOperand.Value) > 0)
                                        result = true;
                                }

                                instruction.Result.Value = result ? 1 : 0;
                                break;

                            case OperationCode.LEq:
                                result = false;
                                if ((instruction.LOperand.Type == VariableType.Int) && (instruction.ROperand.Type == VariableType.Int))
                                {
                                    if ((int)instruction.LOperand.Value <= (int)instruction.ROperand.Value)
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    if (((double)instruction.ROperand.Value - (double)instruction.LOperand.Value + 0.00000001) > 0)
                                        result = true;
                                }

                                instruction.Result.Value = result ? 1 : 0;
                                break;

                            case OperationCode.Gtr:
                                result = false;
                                if ((instruction.LOperand.Type == VariableType.Int) && (instruction.ROperand.Type == VariableType.Int))
                                {
                                    if ((int)instruction.LOperand.Value > (int)instruction.ROperand.Value)
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    if (((double)instruction.LOperand.Value - (double)instruction.ROperand.Value) > 0)
                                        result = true;
                                }

                                instruction.Result.Value = result ? 1 : 0;
                                break;

                            case OperationCode.GEq:
                                result = false;
                                if ((instruction.LOperand.Type == VariableType.Int) && (instruction.ROperand.Type == VariableType.Int))
                                {
                                    if ((int)instruction.LOperand.Value >= (int)instruction.ROperand.Value)
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    if (((double)instruction.LOperand.Value - (double)instruction.ROperand.Value + 0.00000001) > 0)
                                        result = true;
                                }

                                instruction.Result.Value = result ? 1 : 0;
                                break;

                            case OperationCode.NEq:
                                result = false;
                                if ((instruction.LOperand.Type == VariableType.Int) && (instruction.ROperand.Type == VariableType.Int))
                                {
                                    if ((int)instruction.LOperand.Value != (int)instruction.ROperand.Value)
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    if (Math.Abs((double)instruction.LOperand.Value - (double)instruction.ROperand.Value) >= 0.00000001)
                                        result = true;
                                }

                                instruction.Result.Value = result ? 1 : 0;
                                break;

                            case OperationCode.Plus:
                                if (instruction.Result.Type == VariableType.Int)
                                {
                                    instruction.Result.Value = (int)instruction.LOperand.Value + (int)instruction.ROperand.Value;
                                }
                                else
                                {
                                    instruction.Result.Value = (double)instruction.LOperand.Value + (double)instruction.ROperand.Value;
                                }
                                break;

                            case OperationCode.Minus:
                                if (instruction.Result.Type == VariableType.Int)
                                {
                                    instruction.Result.Value = (int)instruction.LOperand.Value - (int)instruction.ROperand.Value;
                                }
                                else
                                {
                                    instruction.Result.Value = (double)instruction.LOperand.Value - (double)instruction.ROperand.Value;
                                }
                                break;

                            case OperationCode.Times:
                                if (instruction.Result.Type == VariableType.Int)
                                {
                                    instruction.Result.Value = (int)instruction.LOperand.Value * (int)instruction.ROperand.Value;
                                }
                                else
                                {
                                    instruction.Result.Value = (double)instruction.LOperand.Value * (double)instruction.ROperand.Value;
                                }
                                break;

                            case OperationCode.DivI:
                                instruction.Result.Value = (int)instruction.LOperand.Value / (int)instruction.ROperand.Value;
                                break;

                            case OperationCode.DivD:
                                instruction.Result.Value = (double)instruction.LOperand.Value / (double)instruction.ROperand.Value;
                                break;

                            case OperationCode.Mod:
                                instruction.Result.Value = (int)instruction.LOperand.Value % (int)instruction.ROperand.Value;
                                break;

                            case OperationCode.UMinus:
                                if (instruction.Result.Type == VariableType.Int)
                                {
                                    instruction.Result.Value = -(int)instruction.LOperand.Value;
                                }
                                else
                                {
                                    instruction.Result.Value = -(double)instruction.LOperand.Value;
                                }
                                break;
                        }
                        break;

                    case InstructionCode.TER:
                        switch (instruction.OperationCode)
                        {
                            case OperationCode.NEq:
                                bool result = false;
                                if ((instruction.LOperand.Type == VariableType.Int) && (instruction.ROperand.Type == VariableType.Int))
                                {
                                    if ((int)instruction.LOperand.Value != (int)instruction.ROperand.Value)
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    if (Math.Abs((double)instruction.LOperand.Value - (double)instruction.ROperand.Value) >= 0.00000001)
                                        result = true;
                                }

                                if (result)
                                    throw new RuntimeException((RuntimeExceptionCode)(int)instruction.Result.Value);
                                break;
                        }
                        break;

                    case InstructionCode.ARI:
                        int index = (int)instruction.ROperand.Value;
                        ((ArrayVariable)(instruction.LOperand)).Index = index;
                        break;

                    case InstructionCode.ARA:
                        int size = (int)instruction.ROperand.Value;
                        ArrayVariable var = (ArrayVariable)(instruction.LOperand);
                        var.Index = -1;
                        switch (instruction.LOperand.Type)
                        {
                            case VariableType.Int:
                                var.Value = new int[size];
                                break;
                            case VariableType.Double:
                                var.Value = new double[size];
                                break;
                            case VariableType.String:
                                var.Value = new string[size];
                                break;
                        }
                        break;

                    case InstructionCode.CAL:
                        if (instruction.LOperand.Value.ToString().CompareTo("LENS") == 0)
                        {
                            string par = code[pc].LOperand.Value.ToString();
                            pc++;

                            instruction.Result.Value = par.Length;
                        }
                        if (instruction.LOperand.Value.ToString().CompareTo("LEN") == 0)
                        {
                            int len = 0;
                            switch (code[pc].LOperand.Type)
                            {
                                case VariableType.Int:
                                    int[] intArray = (int[])code[pc].LOperand.Value;
                                    len = intArray.Length;
                                    break;
                                case VariableType.Double:
                                    double[] doubleArray = (double[])code[pc].LOperand.Value;
                                    len = doubleArray.Length;
                                    break;
                                case VariableType.String:
                                    string[] stringArray = (string[])code[pc].LOperand.Value;
                                    len = stringArray.Length;
                                    break;
                            }
                            pc++;

                            instruction.Result.Value = len;
                        }
                        if (instruction.LOperand.Value.ToString().CompareTo("SQRT") == 0)
                        {
                            double par = (double)code[pc].LOperand.Value;
                            pc++;

                            instruction.Result.Value = Math.Sqrt(par);
                        }
                        if (instruction.LOperand.Value.ToString().CompareTo("RND") == 0)
                        {
                            Random rnd = new Random();
                            instruction.Result.Value = rnd.NextDouble();
                        }
                        if (instruction.LOperand.Value.ToString().CompareTo("GETBASES") == 0)
                        {
                            string starSystem = null;
                            if (code[pc].InstructionCode == InstructionCode.PAR)
                            {
                                starSystem = (string)code[pc].LOperand.Value;
                                pc++;
                            }

                            instruction.Result.Value = new string[] { "base1", "base2", "base3" };
                        }
                        if (instruction.LOperand.Value.ToString().CompareTo("GETPLANETS") == 0)
                        {
                            string starSystem = null;
                            if (code[pc].InstructionCode == InstructionCode.PAR)
                            {
                                starSystem = (string)code[pc].LOperand.Value;
                                pc++;
                            }

                            instruction.Result.Value = new string[] { "merkur", "venuse", "zeme" };
                        }
                        if (instruction.LOperand.Value.ToString().CompareTo("GETSTARSYSTEMS") == 0)
                        {
                            instruction.Result.Value = new string[] { "slunce", "alfa centaury" };
                        }
                        if (instruction.LOperand.Value.ToString().CompareTo("GETEXITS") == 0)
                        {
                            instruction.Result.Value = new string[] { "exit1", "exit2" };
                        }
                        if (instruction.LOperand.Value.ToString().CompareTo("GETPRICE") == 0)
                        {
                            string productName = (string)code[pc].LOperand.Value;
                            pc++;

                            string from = null;
                            if (code[pc].InstructionCode == InstructionCode.PAR)
                            {
                                from = (string)code[pc].LOperand.Value;
                                pc++;
                            }

                            instruction.Result.Value = 128;
                        }
                        if (instruction.LOperand.Value.ToString().CompareTo("GETFUEL") == 0)
                        {
                            instruction.Result.Value = 18;
                        }
                        if (instruction.LOperand.Value.ToString().CompareTo("GETWEAR") == 0)
                        {
                            instruction.Result.Value = 32;
                        }
                        if (instruction.LOperand.Value.ToString().CompareTo("GETSPACE") == 0)
                        {
                            string productName = (string)code[pc].LOperand.Value;
                            pc++;

                            instruction.Result.Value = 6;
                        }
                        if (instruction.LOperand.Value.ToString().CompareTo("GETFLYTIME") == 0)
                        {
                            string productName = (string)code[pc].LOperand.Value;
                            pc++;

                            instruction.Result.Value = 138;
                        }
                        if (instruction.LOperand.Value.ToString().CompareTo("GETSUPPLY") == 0)
                        {
                            string productName = (string)code[pc].LOperand.Value;
                            pc++;

                            string from = null;
                            if (code[pc].InstructionCode == InstructionCode.PAR)
                            {
                                if (code[pc].LOperand != null)
                                {
                                    from = (string)code[pc].LOperand.Value;
                                }
                                pc++;
                            }

                            string inWhat = null;
                            if (code[pc].InstructionCode == InstructionCode.PAR)
                            {
                                if (code[pc].LOperand != null)
                                {
                                    inWhat = (string)code[pc].LOperand.Value;
                                }
                                pc++;
                            }

                            instruction.Result.Value = 140;
                        }
                        break;
                }
            }
        }
    }
}
