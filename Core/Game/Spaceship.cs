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
