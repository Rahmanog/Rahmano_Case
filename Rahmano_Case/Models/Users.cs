using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Rahmano_Case.Models
{
    public class Users
    {
        [Key]
        public int User_ID { get; set; }

        [Required]
        [DisplayName("Name")]
        public string User_Name { get; set; }

        [Required]
        [DisplayName("Sex")]
        public string User_Sex { get; set; }

        [DisplayName("Birth Date")]
        public DateTime User_BirthDate { get; set; }

        [DisplayName("Photo")]
        public byte[] User_Photo{ get; set; }

        [Required]
        [DisplayName("Login Name")]
        public string User_Login { get; set; }

        [Required]
        [DisplayName("Email Address")]
        public string User_Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [MembershipPassword(MinRequiredNonAlphanumericCharacters = 1, MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).", ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!, @, #, etc).")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string User_Pwd { get; set; }
        public string Is_Active { get; set; }
        public string User_GUID { get; set; }

        public string Is_Deleted { get; set; }
        public Users()
        {
            User_ID = 0;
            User_Sex = "M";
            Is_Active = "1";
        }
    }

    public class UserLogin
    {
        [Required]
        [DisplayName("Login Name")]
        public string User_Login { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [MembershipPassword(MinRequiredNonAlphanumericCharacters = 1, MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).", ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!, @, #, etc).")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string User_Pwd { get; set; }
    }

    public class UserPassword
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [MembershipPassword(MinRequiredNonAlphanumericCharacters = 1, MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).", ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!, @, #, etc).")]
        [DataType(DataType.Password)]
        [DisplayName("Old Password")]
        public string Old_pwd { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [MembershipPassword(MinRequiredNonAlphanumericCharacters = 1, MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).", ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!, @, #, etc).")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string User_Pwd { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [MembershipPassword(MinRequiredNonAlphanumericCharacters = 1, MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).", ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!, @, #, etc).")]
        [DataType(DataType.Password)]
        [Compare("User_Pwd")]
        [DisplayName("Confirm Password")]
        public string User_Pwdr { get; set; }
    }

    public class UserSignin
    {
        [Required]
        [DisplayName("Name")]
        public string User_Name { get; set; }

        [Required]
        [DisplayName("Login Name")]
        public string User_Login { get; set; }

        [Required]
        [DisplayName("Email Address")]
        public string User_Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [MembershipPassword(MinRequiredNonAlphanumericCharacters = 1, MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).", ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!, @, #, etc).")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string User_Pwd { get; set; }
    }
}