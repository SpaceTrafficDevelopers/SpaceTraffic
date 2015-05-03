using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipBasicInterpreter.Memory
{
    public interface IOperand
    {
        VariableType Type { get; }

        object Value { get; set; }

        void ResetValue();
    }
}
