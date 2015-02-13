using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SpaceTraffic.Entities
{
    [DataContract(Name = "PlayerInfo")]
    public class Player
    {
        [DataMember]
        public int PlayerId { get; set; }

        [DataMember]
        public string PlayerName { get; set; }      

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PsswdHash { get; set; }

        public string PsswdSalt { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string OrionEmail { get; set; }

        public bool IsFavStudent { get; set; }

        public bool IsOrionEmailConfirmed { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public bool IsAccountLocked { get; set; }       

        public DateTime AddedDate { get; set; }

        public DateTime LastVisitedDate { get; set; }

        public string CorporationName { get; set; }

        public int Credit { get; set; }       

        public virtual ICollection<SpaceShip> SpaceShips { get; set; }  
    }
}
