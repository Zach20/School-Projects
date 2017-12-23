using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections;
using System.Threading.Tasks;
using Bewander.Models;
using Bewander.DAL;
using Bewander.Controllers;
using Microsoft.AspNet.Identity;
using Bewander.ViewModels;

namespace Bewander.Hubs
{
    [HubName("AppHub")]
    public class AppHub : Hub
    {
        private BewanderContext db = new BewanderContext();

        public void RegisterConn(string userID)
        { 
            try
            {
                User user = db.Users.SqlQuery("dbo.User_Select @p0", userID).SingleOrDefault();
                user.ConnectionID = Context.ConnectionId;
                user.Connected = true;
                db.SaveChanges();
                SendListOfConnected(user.UserID);
            }
            catch (Exception)
            {
                Clients.Caller.showErrorMessage("Sorry, could not connect");
            }         

        }

        public void SendListOfConnected(string ID)
        {
            //Send the list to all clients for display
            List<Relationship> friends = db.Relationships.Where(i => (i.UserOneID == ID || i.UserTwoID == ID) && i.Status == RelationshipStatus.Friends).ToList();
            List<ChatViewModel> friendsOnline = new List<ChatViewModel>();
            

            foreach (Relationship friend in friends)
            {
                if (friend.UserOneID != ID)
                {
                    User user = db.Users.Where(i => i.UserID == friend.UserOneID).FirstOrDefault();
                    ChatViewModel chatViewModel = new ChatViewModel(user, friend.ID);
                    friendsOnline.Add(chatViewModel);
                }
                else
                {
                    User user = db.Users.Where(i => i.UserID == friend.UserTwoID).FirstOrDefault();
                    ChatViewModel chatViewModel = new ChatViewModel(user, friend.ID);
                    friendsOnline.Add(chatViewModel);
                }
            }

            Clients.Caller.displayListOfConnected(friendsOnline.OrderBy(i => i.Name));
            // Not all


        } // End SendListOfConnected

        //Method for sending a message to one user
        public void SendMessage(string message, int relationshipID, string userID) 
        {
            Relationship relationship = db.Relationships.Find(relationshipID);
            User targetUser = (relationship.UserOneID != userID) ?
                targetUser = db.Users.SqlQuery("dbo.User_Select @p0", relationship.UserOneID).SingleOrDefault() :
                targetUser = db.Users.SqlQuery("dbo.User_Select @p0", relationship.UserTwoID).SingleOrDefault();

            int status = (int)relationship.Status;
            int messageCount= db.Messages.Where(i => (i.RelationshipID == relationshipID && i.UserID == userID)&&(relationship.UserTwoDateTime<i.DateTime)).Count();
            if (status != 4 || messageCount < 4)
            {
                try
                {
                    string targetConnID = targetUser.ConnectionID;
                    Message newMessage = new Message(relationshipID, message, userID);
                    Clients.Client(targetConnID).receiveMessage(message, relationshipID, userID);
                    Clients.Caller.receiveMessage(message, relationshipID, userID);
                    db.Messages.Add(newMessage);
                    db.Relationships.Find(relationshipID).Messages.Add(newMessage);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //message = "DELIVERY FAILED " + targetUser.FirstName + " is not connected.";
                    Clients.Caller.receiveMessage(message, relationshipID, userID);
                }
            } 
        }
        
        //  Function for when a user disconnects from site
        public override Task OnDisconnected(bool stopCalled)
        {
            //User user = db.Users.Where(i => i.ConnectionID == Context.ConnectionId).SingleOrDefault();
            //user.Connected = false;
            //db.Users.SqlQuery("dbo.User_Update @Connected", false);
            //db.SaveChanges();
            return base.OnDisconnected(stopCalled);
        }
    } // End Hub
}