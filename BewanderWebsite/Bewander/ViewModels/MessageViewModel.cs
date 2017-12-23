using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bewander.DAL;
using Bewander.Models;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Web.Hosting;

namespace Bewander.ViewModels
{

    public class TargetUser
    {
        public string UserID { get; set; }
        
        public string Name { get; set; }

        public TargetUser(string userID, string fName, string lName)
        {
            UserID = userID;
            Name = fName + " " + lName;
        }
    }

    public class ChatboxViewModel : ViewModelDbContext
    {
        public int RelationshipID { get; set; }
        
        public TargetUser TargetUser { get; set; }
        
        public List<Message> Messages { get; set; }

        public RelationshipStatus relationshipStatus { get; set; }

        // Constructors ////////
        public ChatboxViewModel() { }

        public ChatboxViewModel(int relationshipID, TargetUser targetUser, List<Message> messages)
        {
            RelationshipID = relationshipID;
            TargetUser = targetUser;
            Messages = messages;
            relationshipStatus = db.Relationships.Find(relationshipID).Status;
        }
    }
}