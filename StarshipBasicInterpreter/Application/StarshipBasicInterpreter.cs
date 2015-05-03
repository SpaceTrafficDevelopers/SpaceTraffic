using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using StarshipBasicInterpreter.Compilation;
using StarshipBasicInterpreter.Interpreter;

namespace StarshipBasicInterpreter.Application
{
    class StarshipBasicInterpreter
    {

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                using (StreamReader sr = new StreamReader(args[0]))
                {
                    String program = sr.ReadToEnd();

                    CodeGenerator gen = new CodeGenerator();
                    gen.Generate(program);

                    if (gen.Errors.Count == 0)
                    {
                        gen.Memory.ResetMemory();

                        try
                        {
                            Interpreter.Interpreter interpreter = new Interpreter.Interpreter(gen.Code);
                            interpreter.Interpret();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Compilation end with " + gen.Errors.Count + " errors.");

                        for (int i = 0; i < gen.Errors.Count; i++)
                        {
                            Console.WriteLine("Line " + gen.Errors[i].Line + " - " + gen.Errors[i].Error);
                        }
                    }

                    Console.ReadKey();
                }
            }
        }
    }
}
