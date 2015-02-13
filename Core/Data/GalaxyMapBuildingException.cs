using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Data
{
    /// <summary>
    /// Exception during building of a GalaxyMap, when map structure cannot be created for whatever reason.
    /// </summary>
    [Serializable]
    public class GalaxyMapBuildingException : ApplicationException
    {
        public GalaxyMapBuildingException() { }
        public GalaxyMapBuildingException(string message) : base(message) { }
        public GalaxyMapBuildingException(string message, Exception inner) : base(message, inner) { }
        protected GalaxyMapBuildingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

    }
}
