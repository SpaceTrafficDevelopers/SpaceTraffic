using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    public class Message
    {
        public int MessageId { get; set; }

        public string From { get; set; }

        public int RecipientPlayerId { get; set; }

        public virtual Player RecipientPlayer { get; set; }

        public string Body { get; set; }

        public string Type { get; set; }

        public string MetaInfo { get; set; }
    }
}
