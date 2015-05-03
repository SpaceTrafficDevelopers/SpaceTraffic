using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipBasicInterpreter.Compilation
{
    public class LabelJumps
    {
        private readonly List<LabelJump> labelJumps;

        public LabelJumps()
        {
            labelJumps = new List<LabelJump>();
        }

        public LabelJump this[int index]
        {
            get
            {
                if ((index >= 0) && (index < labelJumps.Count))
                {
                    return labelJumps[index];
                }

                return null;
            }
        }

        public int Count
        {
            get
            {
                return labelJumps.Count;
            }
        }

        public void Clear()
        {
            labelJumps.Clear();
        }

        public LabelJump GetLabelJump(string identifier)
        {
            for (int i = 0; i < labelJumps.Count; i++)
            {
                if (identifier.CompareTo((labelJumps[i]).LabelName) == 0)
                    return labelJumps[i];
            }

            LabelJump labelJump = new LabelJump(identifier);
            labelJumps.Add(labelJump);
            return labelJump;
        }
    }
}
