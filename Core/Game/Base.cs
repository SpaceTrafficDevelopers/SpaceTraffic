using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game
{
    public class Base: IGameObject, IVersionedObject
    {
        private Dictionary<int, Spaceship> ships; 

        public int Id { get; set; }

        public Planet Planet { get; set; }

        public DateTime LastUpdate { get; set; }

        public Base(int id, Planet planet)
        {
            this.Id = id;
            this.Planet = planet;
        }

        public bool Add(Spaceship ship)
        {
            throw new NotImplementedException();
        }

        public void Remove(Spaceship ship)
        {
            throw new NotImplementedException();
        }

        DateTime IVersionedObject.LastUpdate
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private System.Threading.ReaderWriterLockSlim _lock = new System.Threading.ReaderWriterLockSlim();

        public System.Threading.ReaderWriterLockSlim Lock
        {
            get { return this._lock; }
        }
    }
}
