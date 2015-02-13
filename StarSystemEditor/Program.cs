using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Tools.StarSystemEditor
{
    /// <summary>
    /// Tato trida slouzi pro spusteni editoru
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Metoda main ktera postupne zavola pripravu, spusteni editoru a pote i testy
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Editor.Preload();
            Tests.Start();           
        }
    }
}
