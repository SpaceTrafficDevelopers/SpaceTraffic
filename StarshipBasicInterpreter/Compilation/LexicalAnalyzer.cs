using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipBasicInterpreter.Compilation
{
    public class LexicalAnalyzer
    {
        private TextReader textReader;
        private char currentChar;

        private Symbols currentSymbol;
        private int currentIntNumber;
        private int currentDecNumber;
        private double currentDoubleValue;
        private bool lastIntTooBig;
        private bool lastDecTooBig;
        private StringBuilder currentIdentifier;
        private StringBuilder currentString;
        private int currentLineNumber;

        private Hashtable keyWords = new Hashtable();
        private Hashtable singleSymbols = new Hashtable();

        public LexicalAnalyzer()
        {
            textReader = new TextReader(string.Empty);
            NextChar();

            singleSymbols.Add(('='), (Symbols.EqlOp));
            singleSymbols.Add(('<'), (Symbols.LessOp));
            singleSymbols.Add(('>'), (Symbols.GtrOp));
            singleSymbols.Add(('('), (Symbols.LParen));
            singleSymbols.Add((')'), (Symbols.RParen));
            singleSymbols.Add(('['), (Symbols.LHParen));
            singleSymbols.Add((']'), (Symbols.RHParen));
            singleSymbols.Add(('*'), (Symbols.TimesOp));
            singleSymbols.Add(('/'), (Symbols.DivideOp));
            singleSymbols.Add(('+'), (Symbols.PlusOp));
            singleSymbols.Add(('-'), (Symbols.MinusOp));
            singleSymbols.Add((','), (Symbols.Comma));

            keyWords.Add(("OR"), (Symbols.OrSym));
            keyWords.Add(("NOT"), (Symbols.NotSym));
            keyWords.Add(("AND"), (Symbols.AndSym));
            keyWords.Add(("LET"), (Symbols.LetSym));
            keyWords.Add(("DIM"), (Symbols.DimSym));
            keyWords.Add(("GO"), (Symbols.GoSym));
            keyWords.Add(("TO"), (Symbols.ToSym));
            keyWords.Add(("IF"), (Symbols.IfSym));
            keyWords.Add(("THEN"), (Symbols.ThenSym));
            keyWords.Add(("ENDIF"), (Symbols.EndIfSym));
            keyWords.Add(("FLY"), (Symbols.FlySym));
            keyWords.Add(("LDCARGO"), (Symbols.LdCargoSym));
            keyWords.Add(("AMNT"), (Symbols.AmntSym));
            keyWords.Add(("FROM"), (Symbols.FromSym));
            keyWords.Add(("ULDCARGO"), (Symbols.UldCargoSym));
            keyWords.Add(("BUY"), (Symbols.BuySym));
            keyWords.Add(("MAXP"), (Symbols.MaxPSym));
            keyWords.Add(("SELL"), (Symbols.SellSym));
            keyWords.Add(("MINP"), (Symbols.MinPSym));
            keyWords.Add(("REPAIR"), (Symbols.RepairSym));
            keyWords.Add(("GET"), (Symbols.GetSym));
            keyWords.Add(("PRICE"), (Symbols.PriceSym));
            keyWords.Add(("BASES"), (Symbols.BasesSym));
            keyWords.Add(("PLANETS"), (Symbols.PlanetsSym));
            keyWords.Add(("STARSYSTEMS"), (Symbols.StarsystemsSym));
            keyWords.Add(("FUEL"), (Symbols.FuelSym));
            keyWords.Add(("SPACE"), (Symbols.SpaceSym));
            keyWords.Add(("FLYTIME"), (Symbols.FlyTimeSym));
            keyWords.Add(("SUPPLY"), (Symbols.SupplySym));
            keyWords.Add(("IN"), (Symbols.InSym));
            keyWords.Add(("EXITS"), (Symbols.ExitsSym));
            keyWords.Add(("WEAR"), (Symbols.WearSym));
            keyWords.Add(("INT"), (Symbols.IntSym));
            keyWords.Add(("FOR"), (Symbols.ForSym));
            keyWords.Add(("STEP"), (Symbols.StepSym));
            keyWords.Add(("NEXT"), (Symbols.NextSym));
            keyWords.Add(("LEN"), (Symbols.LenSym));
            keyWords.Add(("LENS"), (Symbols.LenSSym));
            keyWords.Add(("REM"), (Symbols.RemSym));
            keyWords.Add(("MOD"), (Symbols.ModSym));
            keyWords.Add(("DIV"), (Symbols.DivSym));
            keyWords.Add(("PRINT"), (Symbols.PrintSym));
            keyWords.Add(("SQRT"), (Symbols.SqrtSym));
            keyWords.Add(("RND"), (Symbols.RndSym));
        }

        public void SetProgramCode(string programCode)
        {
            textReader = new TextReader(programCode);

            currentLineNumber = 1;

            NextChar();
        }

        public Symbols NextLine()
        {
            while ((currentChar != '\n') && (currentChar != '\0'))
            {
                NextChar();
            }

            if (currentChar == '\0')
                return Symbols.EndOfProgram;

            NextChar();
            return NextSymbol();
        }

        public Symbols NextSymbol()
        {
            while (currentChar <= ' ')
            {
                if (currentChar == '\n')
                {
                    NextChar();
                    currentLineNumber++;
                    return Symbols.EndOfLine;
                }
                if (currentChar == '\0')
                {
                    return Symbols.EndOfProgram;
                }

                NextChar();
            }

            if (Char.IsLetter(currentChar))
            {
                currentIdentifier = new StringBuilder();
                do
                {
                    currentIdentifier.Append((currentChar));
                    NextChar(); 
                } while (Char.IsLetterOrDigit(currentChar));

                if (keyWords.ContainsKey((string)currentIdentifier.ToString()) == true)
                {
                    currentSymbol = (Symbols)keyWords[(string)currentIdentifier.ToString()];

                    if (currentSymbol == Symbols.RemSym)
                    {
                        return NextLine();
                    }
                }
                else
                {
                    if (currentChar == '%')
                    {
                        currentSymbol = Symbols.IntIdent;
                        currentIdentifier.Append((currentChar));
                        NextChar();

                        if (currentChar == '[')
                        {
                            currentSymbol = Symbols.IntArrayIdent;
                            currentIdentifier.Append((currentChar));
                            NextChar();
                        }
                    }
                    else if (currentChar == '#')
                    {
                        currentSymbol = Symbols.DoubleIdent;
                        currentIdentifier.Append((currentChar));
                        NextChar();

                        if (currentChar == '[')
                        {
                            currentSymbol = Symbols.DoubleArrayIdent; 
                            currentIdentifier.Append((currentChar));
                            NextChar();
                        }
                    }
                    else if (currentChar == '$')
                    {
                        currentSymbol = Symbols.StringIdent;
                        currentIdentifier.Append((currentChar));
                        NextChar();

                        if (currentChar == '[')
                        {
                            currentSymbol = Symbols.StringArrayIdent;
                            currentIdentifier.Append((currentChar));
                            NextChar();
                        }
                    }
                    else if (currentChar == ':')
                    {
                        currentSymbol = Symbols.LabelIdent;
                        NextChar();
                    }
                    else
                    {
                        currentSymbol = Symbols.NoTypeIdent;
                    }
                }
            }
            else if (currentChar == '"')
            {
                NextChar();

                currentString = new StringBuilder();

                while (currentChar != '"')
                {
                    currentString.Append(currentChar);
                    NextChar();
                }

                NextChar();

                currentSymbol = Symbols.String;
            }
            else if (currentChar == '.')
            {
                NextChar();

                if (Char.IsDigit(currentChar))
                {
                    currentSymbol = Symbols.DoubleNumber;
                    currentDecNumber = 0;
                    long cislo = 0;
                    double len = 1;

                    lastDecTooBig = false;
                    do
                    {
                        cislo = (10 * cislo) + (currentChar - '0');
                        len *= 10;
                        NextChar();

                        if ((cislo > int.MaxValue) ||
                             (cislo < int.MinValue))
                            lastDecTooBig = true;
                    } while (Char.IsDigit(currentChar));

                    if (!lastDecTooBig)
                    {
                        currentDecNumber = (int)cislo;

                        currentDoubleValue = cislo / len;
                    }
                }
                else
                {
                    currentSymbol = Symbols.NoSym;
                }
            }
            else if (Char.IsDigit(currentChar))
            {
                currentSymbol = Symbols.IntNumber;
                currentIntNumber = 0;
                long cislo = 0;

                lastIntTooBig = false;
                do
                {
                    cislo = (10 * cislo) + (currentChar - '0');
                    NextChar();

                    if ((cislo > int.MaxValue) ||
                         (cislo < int.MinValue))
                        lastIntTooBig = true;
                } while (Char.IsDigit(currentChar));

                if (!lastIntTooBig)
                    currentIntNumber = (int)cislo;

                if (currentChar == '.')
                {
                    currentSymbol = Symbols.DoubleNumber;

                    NextChar();

                    currentDecNumber = 0;
                    cislo = 0;
                    double len = 1;
                    lastDecTooBig = false;

                    while (Char.IsDigit(currentChar))
                    {
                        cislo = (10 * cislo) + (currentChar - '0');
                        len *= 10;
                        NextChar();

                        if ((cislo > int.MaxValue) ||
                                (cislo < int.MinValue))
                            lastDecTooBig = true;
                    }

                    if ((!lastDecTooBig) && (!lastIntTooBig))
                    {
                        currentDecNumber = (int)cislo;

                        currentDoubleValue = currentIntNumber + cislo / len;
                    }
                }
            }
            else if (currentChar == '<')
            {
                NextChar();
                if (currentChar == '=')
                {
                    currentSymbol = Symbols.LessEqlOp;  /* mensi nebo rovno */
                    NextChar();
                }
                else if (currentChar == '>')
                {
                    currentSymbol = Symbols.NotEqlOp;  /* mensi nebo rovno */
                    NextChar();
                }
                else
                {
                    currentSymbol = Symbols.LessOp;  /* mensi nez */
                }
            }
            else if (currentChar == '>')
            {
                NextChar();
                if (currentChar == '=')
                {
                    currentSymbol = Symbols.GtrEqOp;  /* vetsi nebo rovno */
                    NextChar();
                }
                else
                {
                    currentSymbol = Symbols.GtrOp;  /* vetsi nez */
                }
            }
            else
            {
                if (singleSymbols.ContainsKey((char)(currentChar)))
                {
                    currentSymbol = (Symbols)singleSymbols[(char)(currentChar)];
                    NextChar();
                }
                else
                {
                    currentSymbol = Symbols.NoSym;
                    NextChar();
                }
            }

            return currentSymbol;
        }

        public Symbols CurrentSymbol
        {
            get { return currentSymbol; }
        }

        public int CurrentIntNumber
        {
            get { return currentIntNumber; }
        }

        public double CurrentDoubleValue
        {
            get { return currentDoubleValue; }
        }

        public string CurrentIdentifier
        {
            get { return currentIdentifier.ToString(); }

        }

        public string CurrentString
        {
            get { return currentString.ToString(); }
        }

        private void NextChar()
        {
            currentChar = textReader.NextChar();
        }

        public int CurrentLineNumber
        {
            get { return currentLineNumber; }
        }

        private class TextReader
        {
            private string programCode;
            private int currentCharIndex;

            internal TextReader(string programCode)
            {
                this.programCode = programCode;
                currentCharIndex = 0;
            }

            internal char NextChar()
            {
                if (programCode.Length > currentCharIndex)
                {
                    currentCharIndex++;
                    return programCode[currentCharIndex - 1];
                }

                return '\0';
            }

            internal void ReturnLastChar()
            {
                currentCharIndex--;
            }
        }
    }
}
