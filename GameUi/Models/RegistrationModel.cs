using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SpaceTraffic.GameUi.Models
{
    public class AccountInfoModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "I am student of ZČU.")]
        public bool IsStudent { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "School e-mail address")]
        public string ZcuEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "I agree with the rules")]
        public bool Rules { get; set; }
    }

    public class PlayerInfoModel
    {

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "Nick name")]
        public string NickName { get; set; }


    }

    public enum RegistrationWizzardStep
    {
        ACCOUNT_INFO,
        PLAYER_INFO,
        COMPLETED
    }

    public class RegistrationModel
    {
        public AccountInfoModel AccountInfo { get; set; }
        public PlayerInfoModel PlayerInfo { get; set; }

        public RegistrationWizzardStep WizzardStep { get; set; }
    }
}