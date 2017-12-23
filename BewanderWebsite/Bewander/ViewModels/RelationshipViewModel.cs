using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Bewander.DAL;
using Bewander.Models;
using System.ComponentModel.DataAnnotations;

namespace Bewander.ViewModels
{
    public class RelationshipViewModel
    {
        private static BewanderContext db = new BewanderContext();

        public string UserID { get; set; }
        public int RelationshipID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HomeTown { get; set; }
        public string ProfilePicture { get; set; }
        public string SuggestionReason { get; set; }

        public RelationshipViewModel() { }

        public RelationshipViewModel(User user, string profilePicture, int relationshipID)
        {
            UserID = user.UserID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            string hometownID = db.UserProfiles.Where(a => a.UserID == UserID).Select(a => a.HomeTown).FirstOrDefault();
            HomeTown = db.Places.Where(a => a.PlaceID == hometownID).Select(a => a.Name).FirstOrDefault();
            RelationshipID = relationshipID;
            ProfilePicture = (profilePicture != "no-image.png") ? user.UserID + "/" + profilePicture : profilePicture;
            SuggestionReason = "";
        }

        public RelationshipViewModel(User user, string profilePicture, int relationshipID, string suggestionReason)
        {
            UserID = user.UserID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            string hometownID = db.UserProfiles.Where(a => a.UserID == UserID).Select(a => a.HomeTown).FirstOrDefault();
            HomeTown = db.Places.Where(a => a.PlaceID == hometownID).Select(a => a.Name).FirstOrDefault();
            RelationshipID = relationshipID;
            ProfilePicture = (profilePicture != "no-image.png") ? user.UserID + "/" + profilePicture : profilePicture;
            SuggestionReason = suggestionReason;
        }

        public static List<RelationshipViewModel> GetRelationshipList(string userID, RelationshipStatus status)
        {
            try
            {
                // Create stored procedure for getting list of friends. 
                List<Relationship> friendsList = db.Relationships.Where(i => (i.UserOneID == userID || i.UserTwoID == userID) && i.Status == status).ToList();

                List<RelationshipViewModel> friends = new List<RelationshipViewModel>();

                foreach (Relationship item in friendsList)
                {
                    User user = (item.UserOneID != userID) ? db.Users.Find(item.UserOneID) : db.Users.Find(item.UserTwoID);

                    Image profilePicture = Image.GetProfileImages(user.UserID, FileType.ProfilePicture);
                    RelationshipViewModel friend = new RelationshipViewModel(user, profilePicture.Path, item.ID);
                    friends.Add(friend);
                }

                return (friends);
            }
            catch (Exception)
            {
                return (new List<RelationshipViewModel>());
                throw new NullReferenceException();
            }

        }

        public static List<string> GetFriendsID(string userID)
        {
            IQueryable<Relationship> friends = db.Relationships.Where(i => (i.UserOneID == userID || i.UserTwoID == userID) && i.Status == RelationshipStatus.Friends);
            List<string> friendsIDList = new List<string>();

            try
            {
                foreach (Relationship item in friends)
                {
                    string friendID = (item.UserOneID != userID) ? item.UserOneID : item.UserTwoID;
                    friendsIDList.Add(friendID);
                }

                return (friendsIDList);
            }
            catch //if there are no friends, automatically add "Bewander ID" as a default friend
            {
                friendsIDList.Add("2277d3ef - 4a45 - 4259 - 9695 - f8d4dde70d48");

                return (friendsIDList);
            }
        }

        public static List<RelationshipViewModel> GetNullRelationshipList(string userID)
        {
            try
            {
                List<User> users = db.Users.ToList();
                UserProfile compareProfile = db.UserProfiles.Where(a => a.UserID == userID).FirstOrDefault();
                List<RelationshipViewModel> suggestedUsers = new List<RelationshipViewModel>();
                List<Relationship> relationships = db.Relationships.SqlQuery("dbo.Relationship_Select @p0", userID).ToList();
                //UserProfile userProfile;
                foreach (var user in users)
                {

                    if (userID != user.UserID)
                    {
                        //suggestedFriendsViewModel.Users.Add(user);
                        Relationship relationship = db.Relationships.SqlQuery(
                            "dbo.Check_Friends @p0, @p1", userID, user.UserID).FirstOrDefault();
                        if (relationship == null || relationship.Status == RelationshipStatus.NoRelationship)
                        {
                            UserProfile userProfile = db.UserProfiles.Where(a => a.UserID == user.UserID).FirstOrDefault();
                            // if both users live in the same place, add to Suggested Friends list
                            if (compareProfile.HomeTown == userProfile.HomeTown)
                            {
                                Image profilePicture = Image.GetProfileImages(user.UserID, FileType.ProfilePicture);
                                string reason = "Because you both live in: " + db.Places.Where(a => a.PlaceID == compareProfile.HomeTown).Select(a => a.Name).FirstOrDefault();
                                RelationshipViewModel friend = new RelationshipViewModel(user, profilePicture.Path, 0, reason);
                                suggestedUsers.Add(friend);
                            }
                            // if both users live in the same place, add to Suggested Friends list
                            else if (compareProfile.LastTraveled == userProfile.LastTraveled && compareProfile.LastTraveled != null)
                            {
                                Image profilePicture = Image.GetProfileImages(user.UserID, FileType.ProfilePicture);
                                string reason = "Because you both visited: " + db.Places.Where(a => a.PlaceID == compareProfile.LastTraveled).Select(a => a.Name).FirstOrDefault();
                                RelationshipViewModel friend = new RelationshipViewModel(user, profilePicture.Path, 0, reason);
                                suggestedUsers.Add(friend);
                            }
                        }
                    }
                    Debug.WriteLine("COUNT: " + suggestedUsers.Count.ToString());
                    if (suggestedUsers != null && suggestedUsers.Count > 5)
                    {
                        break;
                    }

                }
                return suggestedUsers;
            }
            catch (Exception)
            {
                return (new List<RelationshipViewModel>());
                throw new NullReferenceException();
            }

        }
        public static List<RelationshipViewModel> GetFriendNotifications(string userID, RelationshipStatus status)
        {
            try
            {
                List<Relationship> friendsList = db.Relationships.Where(i => i.UserTwoID == userID && i.Status == status).ToList();

                List<RelationshipViewModel> friends = new List<RelationshipViewModel>();

                foreach (Relationship item in friendsList)
                {
                    User user = (item.UserOneID != userID) ? db.Users.Find(item.UserOneID) : db.Users.Find(item.UserTwoID);

                    Image profilePicture = Image.GetProfileImages(user.UserID, FileType.ProfilePicture);
                    RelationshipViewModel friend = new RelationshipViewModel(user, profilePicture.Path, item.ID);
                    friends.Add(friend);
                }

                return (friends);
            }
            catch (Exception)
            {
                return (new List<RelationshipViewModel>());
                throw new NullReferenceException();
            }

        }

    }
}
