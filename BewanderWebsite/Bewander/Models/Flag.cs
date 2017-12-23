using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
namespace Bewander.Models
{
    [Table("Flag")]

    public class Flag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostID { get; set; }

        public int FlagStatus { get; set; }

        // Constructors
        public Flag() { }

        public Flag(int postID)
        {
            PostID = postID;
            FlagStatus = 1;
        }
    }
}