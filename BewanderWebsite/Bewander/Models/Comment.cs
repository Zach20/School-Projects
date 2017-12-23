using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bewander.Models
{
    [Table("Comment")]
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Content { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public int Level { get; set; }

        public int Flag { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DatePosted { get; set; }

        public int PostID { get; set; }

        public virtual Post Post { get; set; }

        public virtual User User { get; set; }

        public Comment()
        {
            Likes = new List<Like>();
            Comments = new List<Comment>();
        }

    }
}