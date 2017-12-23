using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bewander.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public string UserID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int Permission { get; set; } // Permisson for admin

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Joined")]
        public DateTime DateJoined { get; set; }

        public string ConnectionID { get; set; } // Connection ID for chat

        public bool Connected { get; set; } // True value for user connected

        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public User() { } // Constructor for empty instance

        public User(string userID, string firstName, string lastName) // Constructor with parameters
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;
            Permission = 0;
            DateJoined = DateTime.Now;
            Files = new List<File>();
            Posts = new List<Post>();
            Reviews = new List<Review>();
        }
    }
}