using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipBasicInterpreter.Compilation
{
    public class CompilationException : Exception
    {
        private readonly int line;
        private readonly ErrorCode error;
        private readonly string description;

        public CompilationException(int line, ErrorCode error, string description)
        {
            this.line = line;
            this.error = error;
            this.description = description;
        }

        public int Line
        {
            get { return line; }
        }

        public ErrorCode Error
        {
            get { return error; }
        }

        public string Description
        {
            get { return description;  }
        }
    }
}
