using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SpaceTraffic.Game
{
    public interface IGameObject
    {
        int Id { get; }

        ReaderWriterLockSlim Lock { get; }
    }
}
