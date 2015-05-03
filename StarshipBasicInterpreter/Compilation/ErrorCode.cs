using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipBasicInterpreter.Compilation
{
    public enum ErrorCode
    {
        ExpectingEOL,
        ExpectingStringExpresion,
        ExpectingStringFactor,
        ExpectingStringArrayFactor,
        ExpectingExpresion,
        ExpectingArrayFactor,
        ExpectingBecomes,
        DoubleIntConversion,
        DifferentArrayTypes,
        ExpectingRParen,
        ExpectingIntModDiv,
        ExpectingFactor,
        ExpectingThen,
        ExpectingIdentifier,
        ExpectingTo,
        NextWrongVal,
        ExpectingLabelIdentifier,
        GoToUnexistingLabel,
        IntWrongParam,
        ExpectingFunction,
        ExpectingRHParen,
        ExpectingArrayIdentifier,
        ExpectingComma,
        ExpectingAmnt,
        ExpectingFrom,
        ExpectingMaxP,
        ExpectingMinP,
        UnexpectingFunction,
        UnknownCommand,
    }
}
