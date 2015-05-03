using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipBasicInterpreter.Interpreter
{
    public class RuntimeException : Exception
    {
        private readonly RuntimeExceptionCode runtimeExceptionCode;

        public RuntimeException(RuntimeExceptionCode runtimeExceptionCode)
        {
            this.runtimeExceptionCode = runtimeExceptionCode;
        }
    }
}
