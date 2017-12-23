using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bewander.Models
{
    [Table("Like")]
    public class Like
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public virtual string UserID { get; set; }

        // Constructors
        public Like() { }

        public Like(string userID)
        {
            UserID = userID;
        }
    }
}