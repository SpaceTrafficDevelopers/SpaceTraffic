﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Engine.Actions
{
    interface IGameAction<T>
    {
        T Perform();
    }
}
