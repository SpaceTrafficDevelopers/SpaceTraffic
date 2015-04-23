﻿/**
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
using SpaceTraffic.Dao;
using SpaceTraffic.Entities;

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

        ITraderCargoDAO GetTraderCargoDAO();

        ITraderDAO GetTraderDAO();
        
        /// <summary>
        /// Return ICargoLoadDao by cargoLoad.
        /// </summary>
        /// <param name="cargoLoad">instance ICargoLoad</param>
        /// <returns>ICargoLoadDao</returns>
        ICargoLoadDao GetCargoLoadDao(string cargoLoadName);

        ITraderDAO GetTraderDAO();
    }
}
