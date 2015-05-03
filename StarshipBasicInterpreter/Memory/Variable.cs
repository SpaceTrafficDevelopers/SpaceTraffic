using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipBasicInterpreter.Memory
{
    public class Variable : IOperand
    {
        private readonly string identifier;
        private readonly VariableType type;
        private object value;

        public Variable(string identifier, VariableType type)
        {
            this.identifier = identifier;
            this.type = type;

            ResetValue();
        }

        public string Identifier
        {
            get { return identifier; }
        }

        public VariableType Type
        {
            get { return type; }
        }

        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public void ResetValue()
        {
            switch (type)
            {
                case VariableType.Int:
                    value = new Int32();
                    break;
                case VariableType.Double:
                    value = new Double();
                    break;
                case VariableType.String:
                    value = string.Empty;
                    break;
            }
        }
    }
}
