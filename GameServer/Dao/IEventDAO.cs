using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public interface IEventDAO
    {
        /// <summary>
        /// Return list of events
        /// </summary>
        /// <returns>List of events</returns>
        List<Event> GetEvents();

        /// <summary>
        /// Return Event by Id
        /// </summary>
        /// <param name="eventId">Event id</param>
        /// <returns>Event</returns>
        Event GetEventById(int eventId);

        /// <summary>
        /// Return list of evenents by 
        /// </summary>
        /// <param name="planet">name</param>
        /// <returns>List of event/returns>
        List<Event> GetEventsBySpaceShipId(int shipId);

        /// <summary>
        /// Insert event to database
        /// </summary>
        /// <param name="event">event</param>
        /// <returns>result of inserting</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool InsertEvent(Event insertedEvent);

        /// <summary>
        /// Update event in database
        /// </summary>
        /// <param name="updatedEvent">updated event</param>
        /// <returns>result of updating</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool UpdateEvent(Event updatedEvent);

        /// <summary>
        /// Remove event from database
        /// </summary>
        /// <param name="eventId">event id</param>
        /// <returns>result of removing</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool RemoveEventById(int eventId);
    }
}
