using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;

namespace StarshipBasicInterpreter.ProgramCode
{
    public class Code
    {
        public List<Instruction> programCode {get; set;}
        private int addressCounter;

        public Code()
        {
            programCode = new List<Instruction>();

            Clear();
        }

        public void Clear()
        {
            programCode.Clear();

            addressCounter = 0;
        }

        public Instruction this[int index]
        {
            get
            {
                if ((index >= 0) && (index < programCode.Count))
                    return programCode[index];

                return null;
            }
        }

        public int Count
        {
            get
            {
                return programCode.Count;
            }
        }

        public int AddressCounter
        {
            get { return addressCounter; }
        }

        public void GenInstruction(InstructionCode instructionCode, OperationCode operationCode, IOperand lOperand, IOperand rOperand, IOperand result)
        {
            programCode.Add(new Instruction(instructionCode, operationCode, lOperand, rOperand, result));

            addressCounter++;
        }

    }
}
