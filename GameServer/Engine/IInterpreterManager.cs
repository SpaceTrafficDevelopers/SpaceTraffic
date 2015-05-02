using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipBasicInterpreter.ProgramCode;

namespace SpaceTraffic.Engine
{
    interface IInterpreterManager
    {
        // name of action runned from starship basic program
        String actionName { get; }

        // action arguments runned from starship basic program
        object[] args { get; }

        // catched exception message
        String catchedExceptionInfo { get; }

        // information if was catched some exception
        bool catchedException { get; }

        void InterpretCode(ref Code currentCode, ref int pc, String playerName,
                           ref Entities.SpaceShip spaceShip,
                           ref Entities.SpaceShipProgram userProgram, IGameServer gameServer,
                           ref bool suspendCurrentCode, ref bool runTooLong);
    }
}
