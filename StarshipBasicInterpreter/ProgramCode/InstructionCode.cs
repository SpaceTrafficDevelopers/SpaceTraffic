using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipBasicInterpreter.ProgramCode
{
    public enum InstructionCode
    {
        NOP,
        STP,    // stop
        WRI,
        OPR,
        STO,
        CST,
        JIF,
        JMP,
        TMO,    // test value and modify operator
        CAL,
        PAR,
        ARI,    // array indexing
        ARA,    // array allocation
        TER,    // runtime error if failed
    }
}
