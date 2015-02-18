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
using SpaceTraffic.Game.Navigation;

namespace SpaceTraffic.Game
{
    /// <summary>
    /// Object representation of starship
    /// Version 1.1
    /// </summary>
    public class Spaceship : SpaceObject, IGameObject, IVersionedObject
    {
        #region Properties
        /// <summary>
        /// ID of starship
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Maximal speed of starship
        /// </summary>
        public int MaxSpeed { get; set; }

        /// <summary>
        /// Path of starship
        /// </summary>
        public NavPath Path { get; set; }

        public NavPoint CurrentNavPoint { get; set; }

        public Base Base { get; set; }

        public bool IsDocked
        {
            get
            {
                return this.Base != null;
            }
        }

        private System.Threading.ReaderWriterLockSlim _lock = new System.Threading.ReaderWriterLockSlim();

        public System.Threading.ReaderWriterLockSlim Lock
        {
            get { return this._lock; }
        }
        #endregion

        #region Constructor

        public Spaceship(int id, string name) : base(name)
        {
            this.Id = id;
        }
        #endregion

        public DateTime LastUpdate { get; set; }
    }
}
