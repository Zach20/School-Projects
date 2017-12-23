using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Bewander.DAL;
using Bewander.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bewander.ViewModels
{
    public class NotificationsViewModel
    {
        private static BewanderContext db = new BewanderContext();
        private static ApplicationDbContext ac = new ApplicationDbContext();



        public int MessageID { get; set; }

        //Use this to flag different message types. 0 = Webmaster Notification, 1 = Administrator Notification, 2 = New User Notification
        public int MessageType { get; set; }

        //Content. We can parse things from the content if needed.
        public string Content { get; set; }

        //Target UserID FOR the notification.
        public string UserID { get; set; }

        //Datetime of the notification.
        public DateTime DateTime { get; set; }
        //Have they read the message? True or False.
        public bool isread { get; set; }
        //Additional data relevant to the notification
        public string data { get; set; }

        public NotificationsViewModel() { }

        public NotificationsViewModel(int messageid)
        {
            MessageID = messageid;
            Content = db.Notifications.Where(i => i.MessageID == messageid).Select(i => i.Content).FirstOrDefault();
            MessageType = db.Notifications.Where(i => i.MessageID == messageid).Select(i => i.MessageType).FirstOrDefault();
            isread = db.Notifications.Where(i => i.MessageID == messageid).Select(i => i.isread).FirstOrDefault();
            data = db.Notifications.Where(i => i.MessageID == messageid).Select(i => i.data).FirstOrDefault();
            DateTime = db.Notifications.Where(i => i.MessageID == messageid).Select(i => i.DateTime).FirstOrDefault();
        }

        //This composes what notifications will be put into the partial view. See _Notifications.cshtml in /Views/Shared
        public static List<NotificationsViewModel> GetNotifications(string userID)
        {

            
            List<Notifications> flags = db.Notifications.Where(i => i.MessageType == 2).ToList();
            var adminID = ac.Roles.Where(i => i.Name == "Admin").SingleOrDefault().Id;
            var admins = ac.Users.Where(u => u.Roles.Any(r => r.RoleId == adminID));
            

            try
            {
                List<Notifications> notifylist = db.Notifications.Where(i => i.UserID == userID).ToList();

                foreach (var admin in admins)
                {
                    if (admin.Id == userID)
                    {
                        foreach (var flag in flags)
                        {
                            notifylist.Add(flag);
                        }
                    }
                    

                    

                }
                        

                List<NotificationsViewModel> notifications = new List<NotificationsViewModel>();

                foreach (Notifications item in notifylist)
                {
                    //We only want to display notifications that aren't already read, and that aren't FOR the currently logged in user...
                    if (item.isread == false && userID != item.data)
                    {

                        NotificationsViewModel notification = new NotificationsViewModel(item.MessageID);


                        notifications.Add(notification);
                    }

                }

                return (notifications);
            }
            catch (Exception)
            {
                return (new List<NotificationsViewModel>());
                throw new NullReferenceException();
            }
        }
    }
}
