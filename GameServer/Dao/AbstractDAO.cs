using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Persistence;

namespace SpaceTraffic.Dao
{
    public class AbstractDAO
    {
        protected SpaceTrafficContext CreateContext()
        {
            return new SpaceTrafficContext();
        }
    }
}
