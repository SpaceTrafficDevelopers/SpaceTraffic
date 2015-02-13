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
