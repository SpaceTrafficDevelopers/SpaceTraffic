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
    public interface IMessageDAO
    {
        /// <summary>
        /// Return message
        /// </summary>
        /// <param name="messageId">message id</param>
        /// <returns>message</returns>
        Message GetMessage(int messageId);

        /// <summary>
        /// Return all messages to player.
        /// </summary>
        /// <param name="playerToId">player id</param>
        /// <returns>list with messages</returns>
        List<Message> GetMessagesToPlayer(int playerToId);

        /// <summary>
        /// Return all messages from player or system with this name.
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>list with messages</returns>
        List<Message> GetMessagesFrom(string name);

        /// <summary>
        /// Insert message to database
        /// </summary>
        /// <param name="message">message</param>
        /// <returns>result of inserting</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool InsertMessage(Message message);

        /// <summary>
        /// Remove message from database
        /// </summary>
        /// <param name="messageId">message id</param>
        /// <returns>result of removing</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool RemoveMessage(int messageId);

    }
}
