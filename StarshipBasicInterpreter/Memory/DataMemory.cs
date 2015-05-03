using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipBasicInterpreter.Memory
{
    public class DataMemory
    {
        private readonly List<IOperand> results;
        private readonly List<IOperand> variables;
        private readonly List<IOperand> arrays;

        public DataMemory()
        {
            results = new List<IOperand>();
            variables = new List<IOperand>();
            arrays = new List<IOperand>();
        }

        public IOperand GetVariable(string identifier, VariableType type)
        {
            if (identifier.EndsWith("["))
            {
                for (int i = 0; i < arrays.Count; i++)
                {
                    if ((identifier.CompareTo(((ArrayVariable)arrays[i]).Identifier) == 0)
                        && (arrays[i].Type == type))
                        return arrays[i];
                }

                IOperand array = new ArrayVariable(identifier, type);
                arrays.Add(array);
                return array;
            }
            else
            {
                for (int i = 0; i < variables.Count; i++)
                {
                    if ((identifier.CompareTo(((Variable)variables[i]).Identifier) == 0)
                        && (variables[i].Type == type))
                        return variables[i];
                }

                IOperand variable = new Variable(identifier, type);
                variables.Add(variable);
                return variable;
            }
        }

        public IOperand GenerateNewResult(VariableType type)
        {
            IOperand result = new IntermediateResult(type);
            results.Add(result);
            return result;
        }

        public IOperand GenerateNewArrayResult(VariableType type)
        {
            IOperand result = new ArrayVariable("-", type);
            results.Add(result);
            return result;
        }

        public void ResetMemory()
        {
            foreach (IOperand operand in results)
            {
                operand.ResetValue();
            }

            foreach (IOperand operand in variables)
            {
                operand.ResetValue();
            }

            foreach (IOperand operand in arrays)
            {
                operand.ResetValue();
            }
        }
    }
}
