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
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public class TraderDAO : AbstractDAO, ITraderDAO
    {
        public List<Trader> GetTraders() {
            using (var contextDB = CreateContext())
            {
                return contextDB.Traders.ToList<Trader>();
            }
        }

        public Trader GetTraderById(int traderId) {
            using (var contextDB = CreateContext())
            {
                return contextDB.Traders.FirstOrDefault(x => x.TraderId.Equals(traderId));
            }
        }

        public Trader GetTraderByBaseId(int baseId) {
            using (var contextDB = CreateContext())
            {
                return contextDB.Traders.Include("Base").FirstOrDefault(x => x.BaseId.Equals(baseId));
            }
        }

        public bool InsertTrader(Trader trader) {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add trader to context
                    contextDB.Traders.Add(trader);
                    // save context to database
                    contextDB.SaveChanges();
                    return true;
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool RemoveTraderById(int traderId) {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var traderTab = contextDB.Traders.FirstOrDefault(x => x.TraderId.Equals(traderId));
                    // remove trader to context
                    contextDB.Traders.Remove(traderTab);
                    // save context to database
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool UpdateTraderById(Trader trader) {
            using (var contextDB = CreateContext())
                try
                {
                    var traderTab = contextDB.Traders.FirstOrDefault(x => x.TraderId.Equals(trader.TraderId));
                    traderTab.BaseId = trader.BaseId;
    
                    // save context to database
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
        }
    }
}
