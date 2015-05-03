using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipBasicInterpreter.Constants;

namespace StarshipBasicInterpreter.Memory
{
    public class ArrayVariable : IOperand
    {
        private readonly string identifier;
        private readonly VariableType type;
        private object value;
        private int index;

        public ArrayVariable(string identifier, VariableType type)
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
            get 
            {
                switch (type)
                {
                    case VariableType.Int:
                        int[] intArray = (int[])this.value;
                        if ((index >= 0) && (index < intArray.Length))
                        {
                            return intArray[index];
                        }
                        break;
                    case VariableType.Double:
                        double[] doubleArray = (double[])this.value;
                        if ((index >= 0) && (index < doubleArray.Length))
                        {
                            return doubleArray[index];
                        }
                        break;
                    case VariableType.String:
                        string[] stringArray = (string[])this.value;
                        if ((index >= 0) && (index < stringArray.Length))
                        {
                            return stringArray[index];
                        }
                        break;
                }

                if (index != -1)
                {
                    throw new IndexOutOfRangeException();
                }

                return this.value; 
            }
            set 
            {
                switch (type)
                {
                    case VariableType.Int:
                        int[] intArray = (int[])this.value;
                        if ((index >= 0) && (index < intArray.Length))
                        {
                            intArray[index] = (int)value;
                            return;
                        }
                        break;
                    case VariableType.Double:
                        double[] doubleArray = (double[])this.value;
                        if ((index >= 0) && (index < doubleArray.Length))
                        {
                            doubleArray[index] = (double)value;
                            return;
                        }
                        break;
                    case VariableType.String:
                        string[] stringArray = (string[])this.value;
                        if ((index >= 0) && (index < stringArray.Length))
                        {
                            stringArray[index] = value.ToString();
                            return;
                        }
                        break;
                }

                if (index != -1)
                {
                    throw new IndexOutOfRangeException();
                }

                this.value = value; 
            }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public void ResetValue()
        {
            switch (type)
            {
                case VariableType.Int:
                    value = new int[0];
                    break;
                case VariableType.Double:
                    value = new double[0];
                    break;
                case VariableType.String:
                    value = new string[0];
                    break;
            }

            index = -1;
        }
    }
}
