using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;

namespace StarshipBasicInterpreter.ProgramCode
{
    public class Instruction
    {
        private InstructionCode instructionCode;
        private OperationCode operationCode;
        private IOperand lOperand;
        private IOperand rOperand;
        private IOperand result;

        public Instruction(InstructionCode instructionCode, OperationCode operationCode, IOperand lOperand, IOperand rOperand, IOperand result)
        {
            this.instructionCode = instructionCode;
            this.operationCode = operationCode;
            this.lOperand = lOperand;
            this.rOperand = rOperand;
            this.result = result;
        }

        public InstructionCode InstructionCode
        {
            get { return instructionCode; }
            set { instructionCode = value; }
        }

        public OperationCode OperationCode
        {
            get { return operationCode; }
            set { operationCode = value; }
        }

        public IOperand LOperand
        {
            get { return lOperand; }
            set { lOperand = value; }
        }

        public IOperand ROperand
        {
            get { return rOperand; }
            set { rOperand = value; }
        }

        public IOperand Result
        {
            get { return result; }
            set { result = value; }
        }
    }
}
