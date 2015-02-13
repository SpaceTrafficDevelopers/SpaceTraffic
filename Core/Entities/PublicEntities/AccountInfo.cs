using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SpaceTraffic.Entities.PublicEntities
{
    [DataContract(Name = "AccountInfo")]
    public class AccountInfo
    {
        [DataMember]
        public string PlayerId { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        public bool IsAccountLocked { get; set; }

        public string PlayerName { get; set; }
    }
}
