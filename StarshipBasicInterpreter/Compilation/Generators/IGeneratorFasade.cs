using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarshipBasicInterpreter.Memory;

namespace StarshipBasicInterpreter.Compilation.Generators
{
    public interface IGeneratorFasade
    {
        IOperand Expresion();

        IOperand Factor();

        IOperand Function();

        IOperand StringExpresion();

        IOperand StringFactor(bool possibleConversion);

        IOperand LogicFactor();

        IOperand Term();

        IOperand AndExpresion();

        IOperand LogicExpresion();

        IOperand LogicTerm();

        void Command();

        void CommadGroup(Symbols returnSymbol);

        Symbols CurrentSymbol { get; }

        void NextSymbol();

        void NextLine();

        IOperand StringArrayFactor();

        IOperand ArrayFactor();
    }
}
