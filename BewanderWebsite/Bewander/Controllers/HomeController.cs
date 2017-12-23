using Microsoft.AspNet.Identity;
using Bewander.DAL;
using Bewander.Models;
using Bewander.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;

namespace Bewander.Controllers
{
    public class HomeController : Controller
    {
        private static BewanderContext db = new BewanderContext();
        //Require all requests must use HTTPS. A more secure approach is to add the RequireHttps filter to the application. 
        //[RequireHttps]
        public ActionResult Index()
        {
            
            return View("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View("About");
        }
        //You can use click on the Contact link to verify anonymous users don't have access and authenticated users do have access.
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View("Contact");
        }

        public ActionResult Help()
        {
            return View("Help");
        }

        public ActionResult FAQ()
        {
            return View("FAQ");
        }

        public ActionResult Privacy()
        {
            return View("Privacy");
        }

        public ActionResult TOS()
        {
            return View("TOS");
        }

        public PartialViewResult Search(string term)
        {
            if(User.Identity.GetUserId() != null)
            {
                List<SearchViewModel> searchList = new List<SearchViewModel>();
                List<User> results = new List<User>();

                if (term.Contains(" "))
                {
                    string[] words = term.Split();
                    results = db.Users.SqlQuery("dbo.User_Search @p0, @p1", words[0], words[1]).ToList();
                }
                else
                {
                    results = db.Users.SqlQuery("dbo.User_Search_One @p0", term).ToList();
                }

                List<Place> places = db.Places.SqlQuery("dbo.Place_Search @p0", term).ToList();

                foreach (User user in results)
                {
                    Image image = Image.GetProfileImages(user.UserID, FileType.ProfilePicture);
                    SearchViewModel result = new SearchViewModel(user.UserID, user.FirstName, user.LastName, image.Path);
                    searchList.Add(result);
                }

                foreach (Place place in places)
                {
                    SearchViewModel result = new SearchViewModel(place);
                    searchList.Add(result);
                }

                return PartialView("_NavbarSearchResults", searchList);
            }

            return PartialView("_NavbarSearchResults", null);
        }

        //-----Friend Icon Menu Search Bar --- Finding Users Only
        public PartialViewResult friendSearch(string term)
        {
            try
            {
                if (User.Identity.GetUserId() != null)
                {
                    List<SearchViewModel> searchList = new List<SearchViewModel>();
                    List<User> results = new List<User>();

                    if (term.Contains(" "))
                    {
                        string[] words = term.Split();
                        results = db.Users.SqlQuery("dbo.User_Search @p0, @p1", words[0], words[1]).ToList();
                    }
                    else
                    {
                        results = db.Users.SqlQuery("dbo.User_Search_One @p0", term).ToList();
                    }

                    foreach (User user in results)
                    {
                        Image image = Image.GetProfileImages(user.UserID, FileType.ProfilePicture);
                        SearchViewModel result = new SearchViewModel(user.UserID, user.FirstName, user.LastName, image.Path);
                        searchList.Add(result);
                    }

                    return PartialView("_NavbarSearchResults", searchList);
                }
            }
            catch (Exception)
            {
                //do nothing
            }
            finally {
             
            }

            return PartialView("_NavbarSearchResults", null);
        }

        public PartialViewResult Notifications()
        {
            //Currently logged in userID
            string userID = User.Identity.GetUserId();
            //Call me old school, but I love me a good ol fashioned sql query. This one will generate the list of notifications for a given userID.
            List<NotificationsViewModel> notifylist = NotificationsViewModel.GetNotifications(userID);
          
    
        
            //db.Notifications.SqlQuery("SELECT * FROM Notifications WHERE UserID=@p0", userID).ToList()
            return PartialView("_Notifications", notifylist);
        }

        public PartialViewResult FriendRequestNotification()
        {
            string userID = User.Identity.GetUserId();
            
            List<RelationshipViewModel> friendRequests = RelationshipViewModel.GetFriendNotifications(userID, RelationshipStatus.Pending);

            return PartialView("_FriendRequests", friendRequests);
        }
        public PartialViewResult FriendList()
        {
            string userID = User.Identity.GetUserId();
            List<RelationshipViewModel> friends = RelationshipViewModel.GetRelationshipList(userID, RelationshipStatus.Friends);
            return PartialView("_FriendList", friends);
        }
        public PartialViewResult MessageNav()
        {
            string userID = User.Identity.GetUserId();
            List<Relationship> relationships = db.Relationships.SqlQuery("dbo.Relationship_Select @p0", userID).ToList();
            List<Message> messages = new List<Message>();
            foreach (Relationship relationship in relationships)
            {
                try
                {
                    if (relationship.UserOneID == userID)
                    {
                        Message message = db.Messages.Where(i => (i.UserID == relationship.UserTwoID) && (i.RelationshipID == relationship.ID)).OrderByDescending(i => i.DateTime).FirstOrDefault();
						if (message != null)
						{
							if (message.DateTime > relationship.UserOneDateTime || relationship.UserOneDateTime == null)
							{
								messages.Add(message);
							}
						}
                    }
                    else
                    {
                        Message message = db.Messages.Where(i => (i.UserID == relationship.UserOneID) && (i.RelationshipID == relationship.ID)).OrderByDescending(i => i.DateTime).FirstOrDefault();

                        if (message.DateTime > relationship.UserTwoDateTime || relationship.UserTwoDateTime == null)
                        {
                            messages.Add(message);
                        }
                    }
                }
                catch { }
            }

            MessageNotificationViewModel messageNotificationViewModel = new MessageNotificationViewModel(messages);
            
            return PartialView("_MessagesNav", messageNotificationViewModel);
        }

        public PartialViewResult _ChatBox(int relationshipID, string targetUserID)
        {
            string userID = User.Identity.GetUserId();

            if (relationshipID == -1)
            {
                try
                {
                    relationshipID = db.Relationships.SqlQuery("dbo.Check_Friends @p0, @p1", userID, targetUserID).FirstOrDefault().ID;
                }
                catch (NullReferenceException)
                {
                    relationshipID = 0;
                }
            }

            relationshipID =  relationshipID == 0 ? createNoRelationship(targetUserID) : relationshipID;

            Relationship relationship = db.Relationships.Find(relationshipID);

            if (relationship.UserOneID != userID)
            {
                targetUserID = relationship.UserOneID;
                relationship.UserTwoDateTime = DateTime.Now;
            }
            else
            {
                targetUserID = relationship.UserTwoID;
                relationship.UserOneDateTime = DateTime.Now;
            }
            db.SaveChanges();

            List<Message> messages = db.Messages.SqlQuery("select * from Message where RelationshipID = @p0", relationshipID).ToList();
            User tuser = db.Users.Find(targetUserID);
            TargetUser targetUser = new TargetUser(tuser.UserID, tuser.FirstName, tuser.LastName);
            var results = new List<PartialViewResult>();
            ChatboxViewModel chatboxViewModel = new ChatboxViewModel(relationshipID, targetUser, messages);
            return PartialView("_ChatBox", chatboxViewModel);
        }

        public int createNoRelationship(string targetUserID)
        {
            string userId = User.Identity.GetUserId();
            if (db.Relationships.SqlQuery("dbo.Check_Friends @p0, @p1", userId, targetUserID).SingleOrDefault() == null)
            {
                Relationship relationship = new Relationship(userId, targetUserID, RelationshipStatus.NoRelationship);
                db.Relationships.Add(relationship);
                db.SaveChanges();
            }
            int relID = db.Relationships.SqlQuery("dbo.Check_Friends @p0, @p1", userId, targetUserID).SingleOrDefault().ID;
            return relID;
        }
    }
}