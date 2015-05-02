using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Constants;
using SpaceTraffic.Engine;
using SpaceTraffic.Utils.Collections;
using SpaceTraffic.GameServer.ServiceImpl;
using StarshipBasicInterpreter.Compilation;
using StarshipBasicInterpreter.Interpreter;
using StarshipBasicInterpreter.ProgramCode;
using StarshipBasicInterpreter.Memory;
using StarshipBasicInterpreter.Constants;
using NLog;

namespace SpaceTraffic.GameServer
{
    public class InterpreterManager : IInterpreterManager
    {
        // name of action runned from starship basic program
        public String actionName { get; private set; }

        private Logger logger = LogManager.GetCurrentClassLogger();

        // action arguments runned from starship basic program
        public object[] args { get; private set; }

        private const double maxRuningTime = 40.0; // time in seconds

        public String catchedExceptionInfo { get; private set; }

        public bool catchedException { get; private set; }

        public InterpreterManager()
        {
            catchedException = false;
            catchedExceptionInfo = "";
        }

        /// <summary>
        /// Interpet program code written by user for selected space ship.
        /// </summary>
        /// <param name="currentCode">current program code</param>
        /// <param name="pc">program counter</param>
        /// <param name="playerName">name of player which own ship</param>
        /// <param name="spaceShip">space ship for which code was written</param>
        /// <param name="userProgram">user program which was written</param>
        /// <param name="gameServer">the game server</param>
        /// <param name="suspendCurrentCode">return information if its long-standing instruction</param>
        /// <param name="runTooLong">return information if complete code runs 
        ///                          longer time than max running time</param>
        public void InterpretCode(ref Code currentCode, ref int pc, String playerName,
                                  ref Entities.SpaceShip spaceShip, 
                                  ref Entities.SpaceShipProgram userProgram, IGameServer gameServer,
                                  ref bool suspendCurrentCode, ref bool runTooLong)
        {
            double startRuntime = gameServer.Game.currentGameTime.ValueInSeconds;
            double runTime = 0.0;
            while ((pc < currentCode.Count) && (pc >= 0) && !suspendCurrentCode
                && !catchedException && !runTooLong)
            {
                Instruction instruction = currentCode[pc];
                pc++;
                switch (instruction.InstructionCode)
                {
                    case InstructionCode.STP:
                        pc = currentCode.Count;
                        break;
                    case InstructionCode.JIF:
                        if ((int)instruction.ROperand.Value == 0)
                            pc = (int)instruction.LOperand.Value;
                        break;
                    case InstructionCode.JMP:
                        pc = (int)instruction.LOperand.Value;
                        break;
                    case InstructionCode.WRI:
                        this.InterpretWriteInstruction(ref instruction, ref spaceShip, ref userProgram);
                        break;
                    case InstructionCode.STO:
                        try
                        {
                            instruction.Result.Value = instruction.LOperand.Value;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            logger.Error("Exception " + ExceptionConstants.IndexOutOfRangeEx + " was caught");
                            catchedException = true;
                            catchedExceptionInfo = ExceptionConstants.IndexOutOfRangeEx;
                            return;
                        }
                        break;
                    case InstructionCode.TMO:
                        if (instruction.ROperand == null) break;
                        this.InterpretTestAndModOpInstruction(currentCode, ref instruction);
                        break;
                    case InstructionCode.CST:
                        this.InterpretCSTInstruction(ref instruction);
                        break;
                    case InstructionCode.OPR:
                        this.InterpretOPRInstruction(ref instruction);
                        break;
                    case InstructionCode.TER:
                        this.RuntimeExChecking(instruction);
                        break;
                    case InstructionCode.ARI:
                        int index = (int)instruction.ROperand.Value;
                        ((ArrayVariable)(instruction.LOperand)).Index = index;
                        break;
                    case InstructionCode.ARA:
                        this.InterpretArrayAllocateInstr(instruction);
                        break;
                    case InstructionCode.CAL:
                        string instructionNamePrefix = instruction.LOperand.Value.ToString().Substring(0, 3);
                        if (instructionNamePrefix.CompareTo("GET") == 0)
                        {
                            this.InterpretGetCALInstruction(ref currentCode, ref pc, ref instruction, playerName, ref spaceShip);
                        }
                        else
                        {
                            this.InterpretCALInstruction(ref currentCode, ref pc, ref instruction, playerName, ref spaceShip, ref suspendCurrentCode);
                        }
                        break;
                }
                // server time has to be updated for difference between current time and start time
                gameServer.Game.currentGameTime.Update();
                runTime = gameServer.Game.currentGameTime.ValueInSeconds - startRuntime;
                // if code was suspended because was runned long-standing instruction is not need check 
                // how long this code is running
                if (!suspendCurrentCode && (runTime >= maxRuningTime)) runTooLong = true;
            }
        }

        /// <summary>
        /// Interprets instruction for array allocation
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        private void InterpretArrayAllocateInstr(Instruction instruction)
        {
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
        }

        /// <summary>
        /// Interprets instruction for checking of runtime exception
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        private void RuntimeExChecking(Instruction instruction)
        {
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
        }

        /// <summary>
        /// Interprets instruction for text printining
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        /// <param name="spaceShip">Space-ship whose code is interpreting</param>
        /// <param name="userProgram">User code which is interpreting</param>
        private void InterpretWriteInstruction(ref Instruction instruction, ref Entities.SpaceShip spaceShip,
            ref Entities.SpaceShipProgram userProgram)
        {
            try
            {
                string currentTime = DateTime.Now.ToString() + " : ";
                string printString = instruction.LOperand.Value.ToString();

                string output = userProgram.programOutput + currentTime + printString + "\n";
                userProgram.programOutput = output;
            }
            catch (IndexOutOfRangeException)
            {
                logger.Error("Exception " + ExceptionConstants.IndexOutOfRangeEx + " was caught");
                catchedException = true;
                catchedExceptionInfo = ExceptionConstants.IndexOutOfRangeEx;
            }
        }

        /// <summary>
        /// Interprets instruction for testing value and operator modification 
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="instruction">Current instruction</param>
        private void InterpretTestAndModOpInstruction(Code currentCode, ref Instruction instruction)
        {
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

            if (instruction.ROperand.Type == VariableType.Int)
            {
                if ((int)instruction.ROperand.Value < 0)
                {
                    currentCode[(int)instruction.LOperand.Value].OperationCode = reverted;
                }
                else
                {
                    currentCode[(int)instruction.LOperand.Value].OperationCode = instruction.OperationCode;
                }
            }
            if (instruction.ROperand.Type == VariableType.Double)
            {
                if ((double)instruction.ROperand.Value < 0)
                {
                    currentCode[(int)instruction.LOperand.Value].OperationCode = reverted;
                }
                else
                {
                    currentCode[(int)instruction.LOperand.Value].OperationCode = instruction.OperationCode;
                }
            }
        }

        /// <summary>
        /// Interprets instructions with instruction code CST
        /// Ref mean reference - for modifiing instruction in method
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        private void InterpretCSTInstruction(ref Instruction instruction)
        {
            switch (instruction.Result.Type)
            {
                case VariableType.Int:
                    try
                    {
                        if (instruction.LOperand.Type == VariableType.Double)
                        {
                            instruction.Result.Value = (int)(double)instruction.LOperand.Value;
                        }
                        else
                        {
                            instruction.Result.Value = instruction.LOperand.Value;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        logger.Error("Exception " + ExceptionConstants.IndexOutOfRangeEx + " was caught");
                        catchedException = true;
                        catchedExceptionInfo = ExceptionConstants.IndexOutOfRangeEx;
                    }
                    break;

                case VariableType.Double:
                    try
                    {
                        if (instruction.LOperand.Type == VariableType.Int)
                        {
                            instruction.Result.Value = (double)(int)instruction.LOperand.Value;
                        }
                        else
                        {
                            instruction.Result.Value = instruction.LOperand.Value;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        logger.Error("Exception " + ExceptionConstants.IndexOutOfRangeEx + " was caught");
                        catchedException = true;
                        catchedExceptionInfo = ExceptionConstants.IndexOutOfRangeEx;
                    }
                    break;

                default:
                    try
                    {
                        instruction.Result.Value = instruction.LOperand.Value.ToString();
                    }
                    catch (IndexOutOfRangeException)
                    {
                        logger.Error("Exception " + ExceptionConstants.IndexOutOfRangeEx + " was caught");
                        catchedException = true;
                        catchedExceptionInfo = ExceptionConstants.IndexOutOfRangeEx;
                    }
                    break;
            }

        }

        /// <summary>
        /// Interprets instructions with instruction code OPR
        /// Ref mean reference - for modifiing instruction in method
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        private void InterpretOPRInstruction(ref Instruction instruction)
        {
            switch (instruction.OperationCode)
            {
                case OperationCode.Conc:
                    instruction.Result.Value = instruction.LOperand.Value.ToString() + instruction.ROperand.Value.ToString();
                    break;

                case OperationCode.And:
                    this.InterpretOpCodeAnd(ref instruction);
                    break;

                case OperationCode.Or:
                    this.InterpretOpCodeOr(ref instruction);
                    break;

                case OperationCode.Not:
                    this.InterpretOpCodeNot(ref instruction);
                    break;

                case OperationCode.Eql:
                    this.InterpretOpCodeEql(ref instruction);
                    break;

                case OperationCode.Lss:
                    this.InterpretOpCodeLess(ref instruction);
                    break;

                case OperationCode.LEq:
                    this.InterpretOpCodeLessEql(ref instruction);
                    break;

                case OperationCode.Gtr:
                    this.InterpretOpCodeGreater(ref instruction);
                    break;

                case OperationCode.GEq:
                    this.InterpretOpCodeGreaterEql(ref instruction);
                    break;

                case OperationCode.NEq:
                    this.InterpretOpCodeNotEql(ref instruction);
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
        }

        /// <summary>
        /// Interprets instructions with instruction code CAL
        /// Ref mean reference - for modifiing instruction in method
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="instruction">Current instruction</param>
        /// <param name="playerName">Player name which runs current code</param>
        /// <param name="spaceShip">Space-ship which current code is interpreting</param>
        /// <param name="suspendCurrentCode">Information if interpreting of code has to be suspended</param>
        private void InterpretCALInstruction(ref Code currentCode, ref int pc, ref Instruction instruction,
                                             string playerName, ref Entities.SpaceShip spaceShip, ref bool suspendCurrentCode)
        {
            if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.StringLengthInst) == 0)
            {
                string par = currentCode[pc].LOperand.Value.ToString();
                pc++;

                instruction.Result.Value = par.Length;
            }
            else if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.ArrayLengthInst) == 0)
            {
                int len = 0;
                switch (currentCode[pc].LOperand.Type)
                {
                    case VariableType.Int:
                        int[] intArray = (int[])currentCode[pc].LOperand.Value;
                        len = intArray.Length;
                        break;
                    case VariableType.Double:
                        double[] doubleArray = (double[])currentCode[pc].LOperand.Value;
                        len = doubleArray.Length;
                        break;
                    case VariableType.String:
                        string[] stringArray = (string[])currentCode[pc].LOperand.Value;
                        len = stringArray.Length;
                        break;
                }
                pc++;

                instruction.Result.Value = len;
            }
            else if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.RadixInst) == 0)
            {
                double par = (double)currentCode[pc].LOperand.Value;
                pc++;

                instruction.Result.Value = Math.Sqrt(par);
            }
            else if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.RandomInst) == 0)
            {
                Random rnd = new Random();
                instruction.Result.Value = rnd.NextDouble();
            }
            /* if other CAL instruction which aren't long-standing will be added
             * should be addded before else part 
             * otherwise should be added to method */
            else
            {
                this.InterpretLongCALInstruction(ref currentCode, ref pc, ref instruction, playerName,
                    ref spaceShip, ref suspendCurrentCode);
            }
        }

        #region Game world getting instructions
        /// <summary>
        /// Interprets instructions with instruction for getting information about game world
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="instruction">Current instruction</param>
        /// <param name="playerName">Player name which runs current code</param>
        /// <param name="spaceShip">Space-ship which current code is interpreting</param>
        private void InterpretGetCALInstruction(ref Code currentCode, ref int pc, ref Instruction instruction,
            string playerName, ref Entities.SpaceShip spaceShip)
        {
            if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.GetBasesInst) == 0)
            {
                this.InterpretGetBasesInstruction(currentCode, ref pc, ref instruction, playerName, spaceShip.SpaceShipId);
            }
            else if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.GetPlanetsInst) == 0)
            {
                this.InterpretGetPlanetsInstruction(currentCode, ref pc, ref instruction, playerName, spaceShip.SpaceShipId);
            }
            else if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.GetStarSystemInst) == 0)
            {
                GameService gs = new GameService();
                IList<Game.StarSystem> starSystems = gs.GetStarSystems();
                string[] starSystemNames = new string[starSystems.Count];
                for (int i = 0; i < starSystemNames.Length; i++) { starSystemNames[i] = starSystems.ElementAt(i).Name; };
                instruction.Result.Value = starSystemNames;
            }
            else if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.GetExitsInst) == 0)
            {
                GameService gs = new GameService();
                Entities.Player shipOwner = gs.GetPlayer(playerName);
                int shipId = spaceShip.SpaceShipId;
                string currentStarsystemName = shipOwner.SpaceShips.FirstOrDefault(s => s.SpaceShipId == shipId).CurrentStarSystem;
                IList<Entities.PublicEntities.WormholeEndpointDestination> exits =
                    gs.GetStarSystemConnections(currentStarsystemName);
                string[] exitNames = new string[exits.Count];
                for (int i = 0; i < exitNames.Length; i++) { exitNames[i] = exits.ElementAt(i).DestinationStarSystemName; };
                instruction.Result.Value = exitNames;
            }
            else if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.GetPriceInst) == 0)
            {
                this.InterpretGetPriceInstruction(currentCode, ref pc, ref instruction, spaceShip);
            }
            else if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.GetFuelInst) == 0)
            {
                instruction.Result.Value = spaceShip.CurrentFuelTank; //TODO overit, ze je to ono
            }
            else if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.GetWearInst) == 0)
            {
                instruction.Result.Value = 32; //TODO: co to ma vracet
            }
            else if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.GetSpaceInst) == 0)
            {
                string productName = (string)currentCode[pc].LOperand.Value;
                pc++;

                instruction.Result.Value = 6;
            }
            else if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.GetFlyTimeInst) == 0)
            {
                this.InterpretGetFlyTimeInstruction(currentCode, ref pc, ref instruction, playerName, spaceShip);
            }
            else if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.GetSupplyInst) == 0)
            {
                this.InterpretGetSupplyInstruction(currentCode, ref pc, ref instruction, spaceShip);
            }
            else
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error("Not supported function for getting info about game world.");
            }

        }
        
        /// <summary>
        /// Interprets instruction for getting information about bases in starsystem
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="instruction">Current instruction</param>
        /// <param name="playerName">Player name which runs current code</param>
        /// <param name="spaceShipId">Id of space-ship which code is interpreting</param>
        private void InterpretGetBasesInstruction(Code currentCode, ref int pc, ref Instruction instruction, string playerName, int spaceShipId)
        {
            GameService gs = new GameService();
            string starSystem = null;
            // if there is specified parameter contain required starsystem
            if (currentCode[pc].InstructionCode == InstructionCode.PAR)
            {
                starSystem = (string)currentCode[pc].LOperand.Value;
                pc++;

                IList<Entities.Base> bases = gs.GetBasesIn(starSystem);
                string[] baseNames = new string[bases.Count];
                // TODO zakladna nema zadne jmeno, co vracet
                for (int i = 0; i < baseNames.Length; i++) { baseNames[i] = bases.ElementAt(i).Planet; };
                instruction.Result.Value = baseNames;
            }
            // if not specified starsystem use current
            else
            {
                IList<Entities.Base> bases = gs.GetBasesInCurrent(playerName, spaceShipId);
                string[] baseNames = new string[bases.Count];
                // TODO zakladna nema zadne jmeno, co vracet
                for (int i = 0; i < baseNames.Length; i++) { baseNames[i] = bases.ElementAt(i).Planet; };
                instruction.Result.Value = baseNames;
            }
        }

        /// <summary>
        /// Interprets instruction for getting information about planets in starsystem
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="instruction">Current instruction</param>
        /// <param name="playerName">Player name which runs current code</param>
        /// <param name="spaceShipId">Id of space-ship which code is interpreting</param>
        private void InterpretGetPlanetsInstruction(Code currentCode, ref int pc, ref Instruction instruction, string playerName, int spaceShipId)
        {
            GameService gs = new GameService();
            string starSystem = null;
            int i = 0;
            // if there is specified parameter contain required starsystem
            if (currentCode[pc].InstructionCode == InstructionCode.PAR)
            {
                starSystem = (string)currentCode[pc].LOperand.Value;
                pc++;

                IKeyAccessibleList<string, Game.Planet> planets = gs.GetPlanetsIn(starSystem);
                string[] planetNames = new string[planets.Count];

                foreach (Game.Planet p in planets)
                {
                    if (p.Name != null)
                    {
                        planetNames[i] = p.Name;
                        i++;
                    }
                }
                instruction.Result.Value = planetNames;
            }
            // if not specified starsystem use current
            else
            {
                IList<Game.Planet> planets = gs.GetPlanetsInCurrent(playerName, spaceShipId);
                string[] planetNames = new string[planets.Count];
                foreach (Game.Planet p in planets)
                {
                    if (p.Name != null)
                    {
                        planetNames[i] = p.Name;
                        i++;
                    }
                }
                instruction.Result.Value = planetNames;
            }
        }

        /// <summary>
        /// Interprets instruction for getting information about price of goods
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="instruction">Current instruction</param>
        /// <param name="playerName">Player name which runs current code</param>
        /// <param name="spaceShip">Space-ship which code is interpreting</param>
        private void InterpretGetPriceInstruction(Code currentCode, ref int pc, ref Instruction instruction, Entities.SpaceShip spaceShip)
        {
            GameService gs = new GameService();

            // get name of product which price player want get
            string productName = (string)currentCode[pc].LOperand.Value;
            pc++;

            string from = null;
            // if there is specified parameter contain required playername
            if (currentCode[pc].InstructionCode == InstructionCode.PAR)
            {
                from = (string)currentCode[pc].LOperand.Value;
                pc++;
                if (!spaceShip.IsFlying)
                {
                    instruction.Result.Value = gs.GetPriceFromPlayer(productName, from);
                }
                else
                {
                    instruction.Result.Value = -1;
                }
            }
            // if not specified playername use price of product on base
            else
            {
                if (!spaceShip.IsFlying)
                {
                    instruction.Result.Value = gs.GetPrice(productName);
                }
                else
                {
                    instruction.Result.Value = -1;
                }
            }
        }

        /// <summary>
        /// Interprets instruction for getting fly time
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="instruction">Current instruction</param>
        /// <param name="playerName">Player name which runs current code</param>
        /// <param name="spaceShip">Space-ship which code is interpreting</param>
        private void InterpretGetFlyTimeInstruction(Code currentCode, ref int pc, ref Instruction instruction, string playerName, Entities.SpaceShip spaceShip)
        {
            GameService gs = new GameService();
            string targetStarSystem = (string)currentCode[pc].LOperand.Value;
            pc++;
            int speed = spaceShip.MaxSpeed;
            int distance = 0;// Int32.MaxValue;

            string currentStarSystem = gs.GetPlayer(playerName).getCurrentStarSystem().Name;

            getEndStarsystem(ref distance, gs, currentStarSystem, targetStarSystem);

            if (distance != -1)
            {
                // TODO: doladit vypocet
                distance = distance * 100;//count of starsystems * xxx
                instruction.Result.Value = distance / speed;
            }
            else
            {
                instruction.Result.Value = -1;
            }
        }

        /// <summary>
        /// Interprets instruction for getting supply goods
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="instruction">Current instruction</param>
        /// <param name="spaceShip">Space-ship which code is interpreting</param>
        private void InterpretGetSupplyInstruction(Code currentCode, ref int pc, ref Instruction instruction, Entities.SpaceShip spaceShip)
        {
            GameService gs = new GameService();
            string productName = (string)currentCode[pc].LOperand.Value;
            pc++;

            string from = null;
            if (currentCode[pc].InstructionCode == InstructionCode.PAR)
            {
                if (currentCode[pc].LOperand != null)
                {
                    from = (string)currentCode[pc].LOperand.Value;
                }
                pc++;
            }

            string inWhat = null;
            if (currentCode[pc].InstructionCode == InstructionCode.PAR)
            {
                if (currentCode[pc].LOperand != null)
                {
                    inWhat = (string)currentCode[pc].LOperand.Value;
                }
                pc++;
            }


            if (!spaceShip.IsFlying)
            {
                //TODO pridat podminku pro in
                if (from != null)
                {
                    instruction.Result.Value = gs.GetSupplyOfGoodsFromPlayer(productName, from);
                }
                else if (inWhat != null)
                {
                    // TODO nahradi GetSupplyOfGoodsFromPlayer(productName, from); za ..In
                    instruction.Result.Value = gs.GetSupplyOfGoodsFromPlayer(productName, from);
                }
                else
                {
                    instruction.Result.Value = gs.GetSupplyOfGoods(productName);
                }
            }
            else
            {
                instruction.Result.Value = -1;
            }
        }

        #endregion

        #region Functions interpret instructions which change some space ship parametr or something in game world
        /// <summary>
        /// Interprets instructions which change some space ship parametr or something in game world
        /// Ref mean reference - for modifiing instruction in method
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="instruction">Current instruction</param>
        /// <param name="playerName">Player name which runs current code</param>
        /// <param name="spaceShip">Space-ship which current code is interpreting</param>
        /// <param name="suspendCurrentCode">Information if interpreting of code has to be suspended</param>
        private void InterpretLongCALInstruction(ref Code currentCode, ref int pc, ref Instruction instruction,
            string playerName, ref Entities.SpaceShip spaceShip, ref bool suspendCurrentCode)
        {
            suspendCurrentCode = true;
            if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.BuyCargoInst) == 0)
            {
                this.InterpretBuyCargoInstruction(currentCode, ref pc, spaceShip);
            }
            if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.SellCargoInst) == 0)
            {
                this.InterpretSellCargoInstruction(currentCode, ref pc, spaceShip);
            }
            if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.LoadCargoInst) == 0)
            {
                this.InterpretLoadCargoInstruction(currentCode, ref pc, spaceShip);
            }
            if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.UnloadCargoInst) == 0)
            {
                this.InterpretUnloadCargoInstruction(currentCode, ref pc, spaceShip);
            }
            // TODO temporary implementation
            if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.FlyToInst) == 0)
            {
                this.InterpretFlyToInstruction(currentCode, ref pc, spaceShip);
            }
            if (instruction.LOperand.Value.ToString().CompareTo(InstructionConstants.RepairShipInst) == 0)
            {
                this.InterpretRepairShipInstruction(currentCode, ref pc, spaceShip);
            }
        }

        /// <summary>
        /// Interprets instructions which call cargo buy
        /// Ref mean reference - for modifiing instruction in method
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="spaceShip">Space-ship which current code is interpreting</param>
        private void InterpretBuyCargoInstruction(Code currentCode, ref int pc, Entities.SpaceShip spaceShip)
        {
            GameService gs = new GameService();
            // get name
            string productName = (string)currentCode[pc].LOperand.Value;
            pc++;

            // get amount
            int amount = 0;
            try
            {
                amount = Convert.ToInt32(currentCode[pc].LOperand.Value);
            }
            catch (ArgumentException ArgEx)
            {
                amount = 0;
                Console.WriteLine(ArgEx.ToString());
            }
            catch (FormatException FoEx)
            {
                amount = 0;
                Console.WriteLine(FoEx.ToString());
            }
            catch (OverflowException OvEx)
            {
                amount = 0;
                Console.WriteLine(OvEx.ToString());
            }

            pc++;

            // get max price
            int maxPrice = 0;
            try
            {
                maxPrice = Convert.ToInt32(currentCode[pc].LOperand.Value);
            }
            catch (ArgumentException ArgEx)
            {
                maxPrice = 0;
                Console.WriteLine(ArgEx.ToString());
            }
            catch (FormatException FoEx)
            {
                maxPrice = 0;
                Console.WriteLine(FoEx.ToString());
            }
            catch (OverflowException OvEx)
            {
                maxPrice = 0;
                Console.WriteLine(OvEx.ToString());
            }
            pc++;

            // get from if this parameter was specified
            string from = null;
            // if there is specified parameter contain required playername
            if (currentCode[pc].InstructionCode == InstructionCode.PAR)
            {
                from = (string)currentCode[pc].LOperand.Value;
                pc++;
                if (!spaceShip.IsFlying)
                {
                    // TODO synchronizovat parametry z programu a existujici
                    args = new object[6] { spaceShip.SpaceShipId, productName, amount, maxPrice, from, "" };
                    actionName = ActionConstants.BuyCargoAction;
                }
                else
                {
                    actionName = null;
                    logger.Error("Can't buy cargo because spaceship is flying.");
                }
            }
            else
            {
                if (!spaceShip.IsFlying)
                {
                    // TODO synchronizovat parametry z programu a existujicimi
                    args = new object[6] { spaceShip.SpaceShipId, productName, amount, maxPrice, "", "" };
                    actionName = ActionConstants.BuyCargoAction;
                }
                else
                {
                    actionName = null;
                    logger.Error("Can't buy cargo because spaceship is flying.");
                }
            }
        }

        /// <summary>
        /// Interprets instructions which call cargo sell
        /// Ref mean reference - for modifiing instruction in method
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="spaceShip">Space-ship which current code is interpreting</param>
        private void InterpretSellCargoInstruction(Code currentCode, ref int pc, Entities.SpaceShip spaceShip)
        {
            GameService gs = new GameService();
            // get name
            string productName = (string)currentCode[pc].LOperand.Value;
            pc++;

            // get amount
            int amount = 0;
            try
            {
                amount = Convert.ToInt32(currentCode[pc].LOperand.Value);
            }
            catch (ArgumentException ArgEx)
            {
                amount = 0;
                Console.WriteLine(ArgEx.ToString());
            }
            catch (FormatException FoEx)
            {
                amount = 0;
                Console.WriteLine(FoEx.ToString());
            }
            catch (OverflowException OvEx)
            {
                amount = 0;
                Console.WriteLine(OvEx.ToString());
            }
            pc++;

            // get min price
            int minPrice = 0;
            try
            {
                minPrice = Convert.ToInt32(currentCode[pc].LOperand.Value);
            }
            catch (ArgumentException ArgEx)
            {
                minPrice = 0;
                Console.WriteLine(ArgEx.ToString());
            }
            catch (FormatException FoEx)
            {
                minPrice = 0;
                Console.WriteLine(FoEx.ToString());
            }
            catch (OverflowException Ex)
            {
                minPrice = 0;
                Console.WriteLine(Ex.ToString());
            }
            pc++;

            // get to if this parameter was specified
            string to = null;
            // if there is specified parameter contain required playername
            if (currentCode[pc].InstructionCode == InstructionCode.PAR)
            {
                to = (string)currentCode[pc].LOperand.Value;
                pc++;
                if (!spaceShip.IsFlying)
                {
                    // TODO synchronizovat parametry z programu a existujici
                    args = new object[6] { spaceShip.SpaceShipId, productName, amount, minPrice, to, "" };
                    actionName = ActionConstants.SellCargoAction;
                }
                else
                {
                    actionName = null;
                    logger.Error("Can't sell cargo because spaceship is flying.");
                }
            }
            else
            {
                if (!spaceShip.IsFlying)
                {
                    // TODO synchronizovat parametry z programu a existujicimi
                    args = new object[6] { spaceShip.SpaceShipId, productName, amount, minPrice, "", "" };
                    actionName = ActionConstants.SellCargoAction;
                }
                else
                {
                    actionName = null;
                    logger.Error("Can't sell cargo because spaceship is flying.");
                }
            }
        }

        /// <summary>
        /// Interprets instructions which call cargo load
        /// Ref mean reference - for modifiing instruction in method
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="spaceShip">Space-ship which current code is interpreting</param>
        private void InterpretLoadCargoInstruction(Code currentCode, ref int pc, Entities.SpaceShip spaceShip)
        {
            GameService gs = new GameService();
            // get name
            string productName = (string)currentCode[pc].LOperand.Value;
            pc++;

            // get amount
            int amount = 0;
            try
            {
                amount = Convert.ToInt32(currentCode[pc].LOperand.Value);
            }
            catch (ArgumentException ArgEx)
            {
                amount = 0;
                Console.WriteLine(ArgEx.ToString());
            }
            catch (FormatException FoEx)
            {
                amount = 0;
                Console.WriteLine(FoEx.ToString());
            }
            catch (OverflowException OvEx)
            {
                amount = 0;
                Console.WriteLine(OvEx.ToString());
            }
            pc++;

            // get from if this parameter was specified
            string from = null;
            // if there is specified parameter contain required playername
            if (currentCode[pc].InstructionCode == InstructionCode.PAR)
            {
                from = (string)currentCode[pc].LOperand.Value;
                pc++;
                if (!spaceShip.IsFlying)
                {
                    // TODO synchronizovat parametry z programu a existujici
                    args = new object[5] { spaceShip.SpaceShipId, productName, amount, from, "" };
                    actionName = ActionConstants.LoadCargoAction;
                }
                else
                {
                    actionName = null;
                    logger.Error("Can't load cargo because spaceship is flying.");
                }
            }
            else
            {
                if (!spaceShip.IsFlying)
                {
                    // TODO synchronizovat parametry z programu a existujicimi
                    args = new object[5] { spaceShip.SpaceShipId, productName, amount, "", "" };
                    actionName = ActionConstants.LoadCargoAction;
                }
                else
                {
                    actionName = null;
                    logger.Error("Can't load cargo because spaceship is flying.");
                }
            }
        }

        /// <summary>
        /// Interprets instructions which call cargo unload
        /// Ref mean reference - for modifiing instruction in method
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="spaceShip">Space-ship which current code is interpreting</param>
        private void InterpretUnloadCargoInstruction(Code currentCode, ref int pc, Entities.SpaceShip spaceShip)
        {
            GameService gs = new GameService();
            // get name
            string productName = (string)currentCode[pc].LOperand.Value;
            pc++;

            // get amount
            int amount = 0;
            try
            {
                amount = Convert.ToInt32(currentCode[pc].LOperand.Value);
            }
            catch (ArgumentException ArgEx)
            {
                amount = 0;
                Console.WriteLine(ArgEx.ToString());
            }
            catch (FormatException FoEx)
            {
                amount = 0;
                Console.WriteLine(FoEx.ToString());
            }
            catch (OverflowException OvEx)
            {
                amount = 0;
                Console.WriteLine(OvEx.ToString());
            }

            pc++;

            // get to if this parameter was specified
            string to = null;
            // if there is specified parameter contain required playername
            if (currentCode[pc].InstructionCode == InstructionCode.PAR)
            {
                to = (string)currentCode[pc].LOperand.Value;
                pc++;
                if (!spaceShip.IsFlying)
                {
                    // TODO synchronizovat parametry z programu a existujici
                    args = new object[5] { spaceShip.SpaceShipId, productName, amount, to, "" };
                    actionName = ActionConstants.UnloadCargoAction;
                }
                else
                {
                    actionName = null;
                    logger.Error("Can't unload cargo because spaceship is flying.");
                }
            }
            else
            {
                if (!spaceShip.IsFlying)
                {
                    // TODO synchronizovat parametry z programu a existujicimi
                    args = new object[5] { spaceShip.SpaceShipId, productName, amount, "", "" };
                    actionName = ActionConstants.UnloadCargoAction;
                }
                else
                {
                    actionName = null;
                    logger.Error("Can't unload cargo because spaceship is flying.");
                }
            }
        }

        /// <summary>
        /// Interprets instructions which call fly to
        /// Ref mean reference - for modifiing instruction in method
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="spaceShip">Space-ship which current code is interpreting</param>
        private void InterpretFlyToInstruction(Code currentCode, ref int pc, Entities.SpaceShip spaceShip)
        {
            GameService gs = new GameService();
            // get name
            string starsystemName = (string)currentCode[pc].LOperand.Value;
            pc++;
            string planetName = (string)currentCode[pc].LOperand.Value;
            pc++;
            string baseName = (string)currentCode[pc].LOperand.Value;
            pc++;

            if (!spaceShip.IsFlying)
            {
                args = new object[5] { spaceShip.SpaceShipId, starsystemName, planetName, baseName, "" };
                actionName = ActionConstants.ShipFlyToAction;
            }
            else
            {
                actionName = null;
                logger.Error("Space ship can't fly because is already flying.");
            }
        }

        /// <summary>
        /// Interprets instructions which repair ship
        /// Ref mean reference - for modifiing instruction in method
        /// </summary>
        /// <param name="currentCode">Current code</param>
        /// <param name="pc">Program counter</param>
        /// <param name="spaceShip">Space-ship which current code is interpreting</param>
        private void InterpretRepairShipInstruction(Code currentCode, ref int pc, Entities.SpaceShip spaceShip)
        {
            GameService gs = new GameService();

            if (!spaceShip.IsFlying)
            {
                args = new object[2] { spaceShip.SpaceShipId, "" };
                actionName = ActionConstants.RepairShipAction;
            }
            else
            {
                actionName = null;
                logger.Error("Can't repair space ship because is flying.");
            }
        }
        #endregion

        #region Operation code instructions
        /// <summary>
        /// Interpret operation code 'AND'
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        private void InterpretOpCodeAnd(ref Instruction instruction)
        {
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
        }

        /// <summary>
        /// Interpret operation code 'OR'
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        private void InterpretOpCodeOr(ref Instruction instruction)
        {
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

            instruction.Result.Value = left || right ? 1 : 0;
        }

        /// <summary>
        /// Interpret operation code 'NOT'
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        private void InterpretOpCodeNot(ref Instruction instruction)
        {
            bool left;
            left = false;
            if ((int)instruction.LOperand.Value != 0)
            {
                left = true;
            }

            instruction.Result.Value = left ? 0 : 1;
        }

        /// <summary>
        /// Interpret operation code '='
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        private void InterpretOpCodeEql(ref Instruction instruction)
        {
            bool result = false;
            try
            {
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
            }
            catch (IndexOutOfRangeException)
            {
                logger.Error("Exception " + ExceptionConstants.IndexOutOfRangeEx + " was caught");
                catchedException = true;
                catchedExceptionInfo = ExceptionConstants.IndexOutOfRangeEx;
            }

            instruction.Result.Value = result ? 1 : 0;
        }

        /// <summary>
        /// Interpret operation code '<'
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        private void InterpretOpCodeLess(ref Instruction instruction)
        {
            bool result = false;
            try
            {
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
            }
            catch (IndexOutOfRangeException)
            {
                logger.Error("Exception " + ExceptionConstants.IndexOutOfRangeEx + " was caught");
                catchedException = true;
                catchedExceptionInfo = ExceptionConstants.IndexOutOfRangeEx;
            }

            instruction.Result.Value = result ? 1 : 0;
        }

        /// <summary>
        /// Interpret operation code '<='
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        private void InterpretOpCodeLessEql(ref Instruction instruction)
        {
            bool result = false;
            try
            {
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
            }
            catch (IndexOutOfRangeException)
            {
                logger.Error("Exception " + ExceptionConstants.IndexOutOfRangeEx + " was caught");
                catchedException = true;
                catchedExceptionInfo = ExceptionConstants.IndexOutOfRangeEx;
            }

            instruction.Result.Value = result ? 1 : 0;
        }

        /// <summary>
        /// Interpret operation code '>'
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        private void InterpretOpCodeGreater(ref Instruction instruction)
        {
            bool result = false;
            try
            {
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
            }
            catch (IndexOutOfRangeException)
            {
                logger.Error("Exception " + ExceptionConstants.IndexOutOfRangeEx + " was caught");
                catchedException = true;
                catchedExceptionInfo = ExceptionConstants.IndexOutOfRangeEx;
            }

            instruction.Result.Value = result ? 1 : 0;
        }

        /// <summary>
        /// Interpret operation code '>='
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        private void InterpretOpCodeGreaterEql(ref Instruction instruction)
        {
            bool result = false;
            try
            {
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
            }
            catch (IndexOutOfRangeException)
            {
                logger.Error("Exception " + ExceptionConstants.IndexOutOfRangeEx + " was caught");
                catchedException = true;
                catchedExceptionInfo = ExceptionConstants.IndexOutOfRangeEx;
            }

            instruction.Result.Value = result ? 1 : 0;
        }

        /// <summary>
        /// Interpret operation code '<>'
        /// </summary>
        /// <param name="instruction">Current instruction</param>
        private void InterpretOpCodeNotEql(ref Instruction instruction)
        {
            bool result = false;
            try
            {
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
            }
            catch (IndexOutOfRangeException)
            {
                logger.Error("Exception " + ExceptionConstants.IndexOutOfRangeEx + " was caught");
                catchedException = true;
                catchedExceptionInfo = ExceptionConstants.IndexOutOfRangeEx;
            }

            instruction.Result.Value = result ? 1 : 0;
        }

        #endregion

        #region Starsystems methods
        /// <summary>
        /// Gets end starsystem
        /// </summary>
        /// <param name="distance">Distance between starstystems</param>
        /// <param name="gs">Game service</param>
        /// <param name="currentStarSystem">Starsystem where is now star-ship</param>
        /// <param name="targetStarSystem">Target starsystem in which star-ship should be docked at the end of fly</param>
        public void getEndStarsystem(ref int distance, GameService gs, string currentStarSystem, string targetStarSystem)
        {
            bool starSystemfounded = false;
            string endStarSystem;
            IList<Entities.PublicEntities.WormholeEndpointDestination> endPoints =
                                      gs.GetStarSystemConnections(currentStarSystem);
            distance++;
            for (int i = 0; i < endPoints.Count(); i++)
            {
                endStarSystem = endPoints.ElementAt(i).DestinationStarSystemName;

                if (targetStarSystem.CompareTo(endStarSystem) == 0)
                {
                    starSystemfounded = true;
                    break;
                }
            }
            if (starSystemfounded)
            {
                return;
            }
            else
            {
                bool starSytemFounded;
                for (int i = 0; i < endPoints.Count(); i++)
                {
                    endStarSystem = endPoints.ElementAt(i).DestinationStarSystemName;
                    starSytemFounded = getEndStarsystemChild(ref distance, gs, endStarSystem, targetStarSystem);
                    if (starSystemfounded) break;
                }
                if (starSystemfounded) return;
                else
                {
                    for (int i = 0; i < endPoints.Count(); i++)
                    {
                        endStarSystem = endPoints.ElementAt(i).DestinationStarSystemName;
                        IList<Entities.PublicEntities.WormholeEndpointDestination> childEndPoints =
                                      gs.GetStarSystemConnections(endStarSystem);
                        for (int j = 0; j < childEndPoints.Count(); j++)
                        {
                            endStarSystem = endPoints.ElementAt(i).DestinationStarSystemName;
                            getEndStarsystem(ref distance, gs, endStarSystem, targetStarSystem);
                            //TODO:co dal //if (starSystemfounded) break;
                        }
                    }
                }
            }
            return;
        }

        /// <summary>
        /// Gets end starsystem child
        /// </summary>
        /// <param name="distance">Distance between starstystems</param>
        /// <param name="gs">Game service</param>
        /// <param name="currentStarSystem">Starsystem where is now star-ship</param>
        /// <param name="targetStarSystem">Target starsystem in which star-ship should be docked at the end of fly</param>
        /// <returns>True if required starsystem was found otherwise false</returns>
        public bool getEndStarsystemChild(ref int distance, GameService gs, string currentStarSystem, string targetStarSystem)
        {
            bool starSystemfounded = false;
            string endStarSystem;
            IList<Entities.PublicEntities.WormholeEndpointDestination> endPoints =
                                      gs.GetStarSystemConnections(currentStarSystem);
            distance++;
            for (int i = 0; i < endPoints.Count(); i++)
            {
                endStarSystem = endPoints.ElementAt(i).DestinationStarSystemName;

                if (targetStarSystem.CompareTo(endStarSystem) == 0)
                {
                    starSystemfounded = true;
                    break;
                }
            }

            return starSystemfounded;
        }

        #endregion
    }
}
