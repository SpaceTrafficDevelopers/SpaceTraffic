using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Dao;

namespace SpaceTraffic.Engine
{
    public interface IPersistenceManager : IDisposable
    {
        //void Initialize();

        //void TestDbConnection();

        IPlayerDAO GetPlayerDAO();

        IMessageDAO GetMessageDAO();

        ICargoDAO GetCargoDAO();

        IFactoryDAO GetFactoryDAO();

        ISpaceShipDAO GetSpaceShipDAO();

        ISpaceShipCargoDAO GetSpaceShipCargoDAO();

        IBaseDAO GetBaseDAO();
    }
}
