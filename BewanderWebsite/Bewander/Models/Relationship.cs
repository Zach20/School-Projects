using Bewander.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bewander.Models
{
    [Table("Relationship")]
    public class Relationship
    {
        // Access database
        //private BewanderContext db = new BewanderContext();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string UserOneID { get; set; }

        public string UserTwoID { get; set; }

        public RelationshipStatus Status { get; set; }

        public bool UserOneOnline { get; set; }

        public bool UserTwoOnline { get; set; }

        public ICollection<Message> Messages { get; set; }

        public DateTime? UserOneDateTime { get; set; }

        public DateTime? UserTwoDateTime { get; set; }

        public Relationship()
        {
            Messages = new List<Message>();
        }

        public Relationship(string userID, string targetUserID, RelationshipStatus status)
        {
            UserOneID = userID;
            UserTwoID = targetUserID;
            Status = status;
            Messages = new List<Message>();
        }


        public static List<int> GetRelationshipIDs(string userID)
        {
            using (BewanderContext db = new BewanderContext())
            {
                List<int> relationshipIDs = db.Relationships.Where(i => i.UserOneID == userID || i.UserTwoID == userID).Select(i => i.ID).ToList();
                return relationshipIDs;
            }
           
        }


    }
}