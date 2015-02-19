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
