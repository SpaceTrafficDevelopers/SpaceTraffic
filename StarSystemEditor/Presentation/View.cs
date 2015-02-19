/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/
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
