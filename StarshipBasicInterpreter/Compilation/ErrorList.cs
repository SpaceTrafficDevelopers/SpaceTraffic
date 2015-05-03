using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipBasicInterpreter.Compilation
{
    public class ErrorList
    {
        private List<CompilationException> errorList;

        public ErrorList()
        {
            errorList = new List<CompilationException>();
        }

        public CompilationException this[int index]
        {
            get
            {
                if ((index >= 0) && (index < errorList.Count))
                    return errorList[index];

                return null;
            }
        }

        public void AddCompilationException(CompilationException exception)
        {
            errorList.Add(exception);
        }
        public int Count
        {
            get { return errorList.Count; }
        }
    }
}
