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

        public ITraderDAO GetTraderDAO()
        {
            return new TraderDAO();
        }

        public ITraderCargoDAO GetTraderCargoDAO()
        {
            return new TraderCargoDAO();
        }

        public ICargoLoadDao GetCargoLoadDao(string cargoLoadDaoName)
        {
            Type classGoodsType = Type.GetType("SpaceTraffic.Dao." + cargoLoadDaoName);
            return (ICargoLoadDao) Activator.CreateInstance(classGoodsType);
        }

        /// <summary>
        /// Gets a new DAO for working with <see cref="GameAction"/> entities.
        /// </summary>
        /// <returns>New instance of <c>IGameActionDAO</c> implementation.</returns>
        public IGameActionDAO GetGameActionDao()
        {
            return new GameActionDAO();
        }

        /// <summary>
        /// Gets a new DAO for working with <see cref="GameEvent"/> entities.
        /// </summary>
        /// <returns>New instance of <c>IGameEventDAO</c> implementation.</returns>
        public IGameEventDAO GetGameEventDao()
        {
            return new GameEventDAO();
        }


        public IPlanActionDAO GetPlanActionDAO()
        {
            return new PlanActionDAO();
        }

        public IPathPlanEntityDAO GetPathPlanEntityDAO()
        {
            return new PathPlanEntityDAO();
        }

        public IPlanItemEntityDAO GetPlanItemEntityDAO()
        {
            return new PlanItemEntityDAO();
        }
    }
}
