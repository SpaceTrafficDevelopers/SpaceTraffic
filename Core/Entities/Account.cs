using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SpaceTraffic.Entities
{
    public class Account
    {
        public string PlayerId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PsswdHash { get; set; }

        public string PsswdSalt { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string SchoolEmail { get; set; }

        public bool IsStudent { get; set; }

        public bool IsSchoolEmailConfirmed { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public bool IsAccountLocked { get; set; }

        public string PlayerName { get; set; }

    }
}
