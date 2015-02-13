using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Engine;

namespace SpaceTraffic.Game.Actions
{
    public class ShipFlyTo : IGameAction
    {
        public GameActionState State
        {
            get { throw new NotImplementedException(); }
        }

        public int PlayerId
        {
            get { throw new NotImplementedException(); }
        }

        public int ActionCode
        {
            get { throw new NotImplementedException(); }
        }

        public object Result
        {
            get { throw new NotImplementedException(); }
        }

        void IGameAction.Perform(IGameServer gameServer)
        {
            throw new NotImplementedException();
        }
    }
}
