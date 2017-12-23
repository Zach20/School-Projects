using Bewander.Models;
using System.Linq;

namespace Bewander.ViewModels
{
    public class ChatViewModel
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string ConnectionID { get; set; }
        public int RelationshipID { get; set; }
        public string ProfilePicPath { get; set; }

        public ChatViewModel() { }

        public ChatViewModel(User user, int relationshipID)
        {
            UserID = user.UserID;
            Name = user.FirstName + " " + user.LastName;
            ConnectionID = user.ConnectionID;
            RelationshipID = relationshipID;
        }
    }
}