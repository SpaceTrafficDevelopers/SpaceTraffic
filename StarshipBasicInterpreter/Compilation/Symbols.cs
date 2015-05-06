﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipBasicInterpreter.Compilation
{
    public enum Symbols
    {
        NoSym = 0,
        EndOfLine,
        EndOfProgram,
        IntNumber,
        DoubleNumber,
        String,
        LabelIdent,
        StringIdent,
        IntIdent,
        DoubleIdent,
        StringArrayIdent,
        IntArrayIdent,
        DoubleArrayIdent,
        NoTypeIdent,
        LParen,
        RParen,
        LHParen, // [
        RHParen, // ]
        Comma,
        EqlOp,
        LessOp,
        GtrOp,
        LessEqlOp,
        GtrEqOp,
        NotEqlOp,
        PlusOp,
        MinusOp,
        TimesOp,
        DivideOp,
        OrSym,
        NotSym,
        AndSym,
        LetSym,
        DimSym,
        GoSym,
        ToSym,
        IfSym,
        ThenSym,
        EndIfSym,
        FlySym,
        LdCargoSym,
        AmntSym,
        FromSym,
        UldCargoSym,
        BuySym,
        MaxPSym,
        SellSym,
        MinPSym,
        RepairSym,
        GetSym,
        PriceSym,
        BasesSym,
        PlanetsSym,
        StarsystemsSym,
        FuelSym,
        SpaceSym,
        FlyTimeSym,
        SupplySym,
        InSym,
        ExitsSym,
        WearSym,
        IntSym,
        ForSym,
        StepSym,
        NextSym,
        SqrtSym,
        PrintSym,
        RndSym,
        ModSym,
        DivSym,
        RemSym,
        LenSym,
        LenSSym
    }
}