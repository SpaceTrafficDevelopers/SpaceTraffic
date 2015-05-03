using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipBasicInterpreter.Compilation
{
    public class LabelJump
    {
        private readonly string labelName;
        private int labelLine;
        private List<int> codeLines;
        private List<int> programLines;

        public LabelJump(string labelName)
        {
            this.labelName = labelName;

            codeLines = new List<int>();
            programLines = new List<int>();
            labelLine = -1;
        }

        public void AddJumpLine(int line)
        {
            codeLines.Add(line);
        }

        public void AddProgramLine(int line)
        {
            programLines.Add(line);
        }

        public string LabelName
        {
            get { return labelName; }
        } 

        public int LabelLine
        {
            get { return labelLine; }
            set { labelLine = value; }
        }

        public int[] ProgramLines
        {
            get
            {
                return programLines.ToArray();
            }
        }

        public int[] CodeLines
        {
            get
            {
                return codeLines.ToArray();
            }
        }
    }
}
