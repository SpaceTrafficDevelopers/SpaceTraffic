using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Engine
{
    interface IGameSimulation
    {
        void Update(GameTime gameTime);
    }
}
