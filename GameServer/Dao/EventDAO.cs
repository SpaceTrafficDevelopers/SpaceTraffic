using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public class EventDAO : AbstractDAO, IEventDAO
    {
        public List<Event> GetEvents()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Events.ToList<Event>();
            }
        }


        public Event GetEventById(int eventId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Events.FirstOrDefault(x => x.EventId.Equals(eventId));
            }
        }


        public List<Event> GetEventsBySpaceShipId(int shipId)
        {
            using (var contextDB = CreateContext())
            {
                return (from x in contextDB.Events
                        where x.SpaceShipId.Equals(shipId)
                        select x).ToList<Event>();
            }
        }


        public bool InsertEvent(Event insertedEvent)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add event to context
                    contextDB.Events.Add(insertedEvent);
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

        public bool UpdateEvent(Event updatedEvent)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var eventTab = contextDB.Events.FirstOrDefault(x => x.EventId.Equals(updatedEvent.EventId));
                    eventTab.Content = updatedEvent.Content;
                    eventTab.CreationedTime = updatedEvent.CreationedTime;
                    eventTab.Type = updatedEvent.Type;
                    eventTab.SpaceShipId = updatedEvent.SpaceShipId;

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

        public bool RemoveEventById(int eventId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var eventTab = contextDB.Events.FirstOrDefault(x => x.EventId.Equals(eventId));
                    // remove event from context
                    contextDB.Events.Remove(eventTab);
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
}
