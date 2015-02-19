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
using System.Data.Entity;
using System.Text;
using System.IO;

namespace SpaceTraffic.Persistence
{
    internal class SpaceTrafficCreateDatabaseIfNotExists : IDatabaseInitializer<SpaceTrafficContext>
    {
        private string scriptPath;

        public SpaceTrafficCreateDatabaseIfNotExists(string scriptPath)
        {
            this.scriptPath = scriptPath;
        }


        public void InitializeDatabase(SpaceTrafficContext context)
        {
            // pokud existuje tak se smaže databáze
            if (!context.Database.Exists())
            {
                context.Database.Create();

                // spustí se script
                if (!string.IsNullOrEmpty(scriptPath))
                {
                    context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_Players_PlayerName ON Players (PlayerName)");
                    context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_Players_Email ON Players (Email)");
                    context.Database.ExecuteSqlCommand(File.ReadAllText(scriptPath));
                }
            }            

           
           
        }

    }
}
