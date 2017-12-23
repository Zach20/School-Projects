using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bewander.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        // Property Values
        [Key]
        public string UserID { get; set; }

        [Display(Name = "Home Town")]
        public string HomeTown { get; set; }

        public string PastLocal { get; set; }

        public string LastTraveled { get; set; }

        public string FavoritePlace { get; set; }

        [DataType(DataType.MultilineText)]
        public string About { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        public bool Private { get; set; }

        public UserProfile() {}

      
    }
}