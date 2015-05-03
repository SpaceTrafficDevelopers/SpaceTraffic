using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipBasicInterpreter.Memory
{
    public class Constant : IOperand
    {
        public readonly VariableType type;
        public readonly object value;

        public Constant(VariableType type, object value)
        {
            this.type = type;
            this.value = value;
        }

        public VariableType Type
        {
            get { return type; }
        }

        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void ResetValue()
        { }
    }
}
