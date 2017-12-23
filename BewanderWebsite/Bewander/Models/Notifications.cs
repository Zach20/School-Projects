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
    [Table("Notifications")]
    public class Notifications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        
        public string data { get; set; }
        public Notifications() { }

        public Notifications(int messagetype, string content, string userID)
        {
            MessageType = messagetype;
            Content = content;
            UserID = userID;
            DateTime = DateTime.Now;
        }
        public Notifications(int messagetype, string content, string userID, string Data)
        {
            MessageType = messagetype;
            Content = content;
            UserID = userID;
            DateTime = DateTime.Now;
            data = Data;
        }
        public static List<int> GetNotifications(string userID)
        {
            using (BewanderContext db = new BewanderContext())
            {
                List<int> notificationIDs = db.Notifications.Where(i => i.UserID == userID).Select(i => i.MessageID).ToList();
                return notificationIDs;
            }

        }
    }
}