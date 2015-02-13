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
