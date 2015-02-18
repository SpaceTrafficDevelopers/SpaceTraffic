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
using System.IO;

namespace SpaceTraffic.Persistence
{
    public class SpaceTrafficCustomInitializer
    {
        public enum InitializeType { DropCreateDatabaseIfModelChanges = 0, DropCreateDatabaseAlways = 1, CreateDatabaseIfNotExists = 2 }

        private string scriptPath;

        public SpaceTrafficCustomInitializer(InitializeType type, string scriptPath)
        {
            this.scriptPath = scriptPath;
            // vybrat typ inicializátoru
            switch(type)
            {
                // odstraní existující databázi a poté vytvoří novou pokud se změní model
                case InitializeType.DropCreateDatabaseIfModelChanges:
                    Database.SetInitializer(new SpaceTrafficDropCreateDatabaseIfModelChanges(scriptPath));               
                    break;
                // odstraní existující databázi a poté vytvoří novou vždy
                case InitializeType.DropCreateDatabaseAlways:
                    Database.SetInitializer(new SpaceTrafficDropCreateDatabaseAlways(scriptPath));
                    break;
                // vytvoří databázi když neexistuje
                case InitializeType.CreateDatabaseIfNotExists:
                    Database.SetInitializer(new SpaceTrafficCreateDatabaseIfNotExists(scriptPath));
                    break;                
            }
          
            // spustit inicializaci 
            using (var db = new SpaceTrafficContext())
            {             

                db.Database.Initialize(true);       
               
                
            }

        }     


        
    }

    



}
