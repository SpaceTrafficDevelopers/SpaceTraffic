using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using StarshipBasicInterpreter.ProgramCode;

namespace SpaceTraffic.Entities
{
    [DataContract(Name = "SpaceShipProgram")]
    public class SpaceShipProgram
    {
        [DataMember]
        public int SpaceShipProgramId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string UserProgram { get; set; }

        [DataMember]
        public DateTime LastModDate { get; set; }

        [DataMember]
        public int SpaceShipId { get; set; }

        //[DataMember]
        public virtual SpaceShip SpaceShip { get; set; }

        public Code code;

        [DataMember]
        public int programCounter { get; set; }

        [DataMember]
        public string programOutput { get; set; }

        private bool programStarted;

        public SpaceShipProgram()
        {
            programStarted = false;
            programOutput = "";
        }

        /*public IList<Instruction> getProgramInstructions()
        {
            return ProgramInstructions;
        }

        public void setProgramInstructions(IList<Instruction> programInstructions)
        {
            ProgramInstructions = programInstructions;
        }*/

        public Code getCode()
        {
            return code;
        }

        public void setCode(Code programCode)
        {
            this.code = programCode;
        }

        public int getProgramCounter()
        {
            return programCounter;
        }

        public void setProgramCounter(int programCounter)
        {
            this.programCounter = programCounter;
        }

        /*public void addOutputLine(string newLine)
        {
            programOutput += newLine;
        }

        public string getProgramOutput()
        {
            return programOutput;
        }*/

        public void setProgramStarted(bool started)
        {
            this.programStarted = started;
        }

        public bool isProgramStarted()
        {
            return programStarted;
        }
    }
}
