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
            throw new NotImplementedException();
        }

        int IComparable.CompareTo(object obj)
        {
            throw new NotImplementedException();
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
