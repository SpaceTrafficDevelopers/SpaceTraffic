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
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SpaceTraffic.Entities
{
    [DataContract(Name = "Base")]
    public class Base
    {
        public int BaseId { get; set; }

        public string Planet { get; set; }

        public Trader Trader { get; set; }

        [DataMember]
        public virtual ICollection<SpaceShip> SpaceShips { get; set; }

        public virtual ICollection<Factory> Factories { get; set; }


        
        public Base()
        {
            SpaceShips = new Collection<SpaceShip>();
        }
        



        public void AddSpaceShip(SpaceShip ship)
        {
            SpaceShips.Add(ship);
        }

        public SpaceShip GetSpaceShip(int shipId)
        {
            //this has O(n)! Dictionary will be better, but it can not be mapped to entity framework
            foreach (SpaceShip s in SpaceShips)
            {
                if (s.SpaceShipId.Equals(shipId))
                {
                    return s;
                }
            }
            return null;
        }

        public bool RemoveSpaceShip(SpaceShip ship)
        {
            return SpaceShips.Remove(ship);
        }
    }
}
