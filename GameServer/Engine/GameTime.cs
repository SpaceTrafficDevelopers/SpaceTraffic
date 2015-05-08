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

namespace SpaceTraffic.Engine
{
    /// <summary>
    /// Reprezentuje herní čas. Vnitřně využívá DateTime pro uchovávání hodnoty času.
    /// Hra využívá převod konkrétního času na počet sekund uplynulých od referenčního času (použit čas 1.1.0001 0:00:00).
    /// </summary>
    public class GameTime : IComparable, IComparable<GameTime>, IEquatable<GameTime>
    {
        public static readonly DateTime REFERENTIAL_TIME = (new DateTime()).ToUniversalTime();

        private DateTime _currentTime;
        private double _seconds;
        
        public double ValueInSeconds
        {
            get
            {
                return _seconds;
            }
        }

        public DateTime Value
        {
            get { return this._currentTime; }
            set
            {
                this._currentTime = value;
                this._seconds = DateTimeToSeconds(value);
            }
        }

        protected internal GameTime()
        {
            this.Value = DateTime.UtcNow;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return this.Value.Equals(((GameTime)obj).Value);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public bool Equals(GameTime other)
        {
            return (other != null) && (this.Value.Equals(other.Value));
        }

        public int CompareTo(GameTime other)
        {
            return _currentTime.CompareTo(other._currentTime);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            if (obj is GameTime)
            {
                return CompareTo((GameTime) obj);
            }
            throw new ArgumentException("GameTime can be compared only to null and other GameTime instance.");
        }

        public static double DateTimeToSeconds(DateTime dateTime)
        {
            return (dateTime.ToUniversalTime() - REFERENTIAL_TIME).TotalSeconds;
        }

        public static DateTime SecondsToDateTime(double seconds)
        {
            return REFERENTIAL_TIME.Add(TimeSpan.FromSeconds(seconds));
        }

        public void Update()
        {
            this.Value = DateTime.UtcNow;
        }
    }
}
