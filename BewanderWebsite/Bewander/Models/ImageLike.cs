using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bewander.Models
{
    public class ImageLike
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageLikeID { get; set; }

        public string UserID { get; set; }
        public int FileID { get; set; }
        
    }
}