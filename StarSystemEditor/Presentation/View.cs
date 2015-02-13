using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using SpaceTraffic.Game;

namespace SpaceTraffic.Tools.StarSystemEditor.Presentation
{
    /// <summary>
    /// Abstraktni trida pouzivana pro ostatni zobrazovace
    /// </summary>
    public abstract class View
    {
        /// <summary>
        /// Abstraktni metoda pro navrat grafiky ke kresleni
        /// </summary>
        /// <returns>Grafiku k vykresleni</returns>
        public abstract Ellipse GetShape();
        /// <summary>
        /// Abstraktni metoda pro ziskani objektu ktery kreslim
        /// </summary>
        /// <returns>Zpracovavany objekt</returns>
        public abstract Object GetLoadedObject();
        /// <summary>
        /// Metoda pro vraceni jmena objektu
        /// </summary>
        /// <returns>Jmeno objektu</returns>
        public abstract String GetName();
        /// <summary>
        /// Metoda pro ziskani velikosti objektu
        /// </summary>
        /// <returns>Velikost objektu</returns>
        public abstract Size GetSize();
    }
}
