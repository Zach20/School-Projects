using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bewander.ViewModels;
using Bewander.DAL;
using Newtonsoft.Json;

namespace Bewander.Models
{
    [Table("Message")]
    public class Message 
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }

        public int? RelationshipID { get; set; }

        public string Content { get; set; }

        public string UserID { get; set; }

        public DateTime DateTime { get; set; }
       
        public Message() { } 

        public Message(int relationshipID, string content, string userID)
        {
            RelationshipID = relationshipID;
            Content = content;
            UserID = userID;
            DateTime = DateTime.Now;
        }
    }
}