using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using SpaceTraffic.Entities;
using SpaceTraffic.Persistence;
using SpaceTraffic.Dao;
using NLog;
using System.Data.Common;
using System.Data.EntityClient;
using System.Configuration;
using System.Data.SqlClient;
using SpaceTraffic.GameServer.Configuration;
using SpaceTraffic.Engine;

namespace SpaceTraffic.GameServer
{
    class PersistenceManager : IPersistenceManager
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public void Initialize()
        {
            
            
            // zavolá se třída DatabaseInitializer            
            new SpaceTrafficCustomInitializer(GameServerConfiguration.GameServerConfig.Initializer.Type, GameServerConfiguration.GameServerConfig.Initializer.InputScript);          
            TestDbConnection();
        }

        public void TestDbConnection()
        {
            logger.Info("Testing database connection.");
            try
            {
                string strConnectionString = ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString;
                DbConnection connection = new SqlConnection(strConnectionString);
                connection.Open();
                connection.Close();
            }
            catch (Exception ex)
            {
                logger.Info("Database connection test failed: {}", ex.Message, ex);
                throw;
            }
            logger.Info("Database connection test successfull");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IPlayerDAO GetPlayerDAO()
        {
            return new PlayerDAO();
        }

        public IMessageDAO GetMessageDAO()
        {
            return new MessageDAO();
        }

        public ICargoDAO GetCargoDAO()
        {
            return new CargoDAO();
        }

        public IFactoryDAO GetFactoryDAO()
        {
            return new FactoryDAO();
        }

        public ISpaceShipDAO GetSpaceShipDAO()
        {
            return new SpaceShipDAO();
        }

        public ISpaceShipCargoDAO GetSpaceShipCargoDAO()
        {
            return new SpaceShipCargoDAO();
        }

        public IBaseDAO GetBaseDAO()
        {
            return new BaseDAO();
        }
    }
}
