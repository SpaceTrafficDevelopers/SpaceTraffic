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
    public class MessageDAO : AbstractDAO, IMessageDAO
    {
        public List<Message> GetMessagesToPlayer(int playerToId)
        {
            using (var contextDB = CreateContext())
            {
                return (from x in contextDB.Messages
                        where x.RecipientPlayerId.Equals(playerToId)
                        select x).ToList();
            }
        }

        public List<Message> GetMessagesFrom(string name)
        {
            using (var contextDB = CreateContext())
            {
                return (from x in contextDB.Messages
                        where x.From.Equals(name)
                        select x).ToList();
            }
        }

        public bool InsertMessage(Message message)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    contextDB.Messages.Add(message);
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool RemoveMessage(int messageId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    Message message = contextDB.Messages.FirstOrDefault(x => x.MessageId.Equals(messageId));
                    contextDB.Messages.Remove(message);
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public Message GetMessage(int messageId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Messages.FirstOrDefault(x => x.MessageId.Equals(messageId));
            }
        }
    }
}
