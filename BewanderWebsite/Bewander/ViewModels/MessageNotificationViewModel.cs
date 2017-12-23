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

    public class MessageNotificationViewModel : ViewModelDbContext
    {
        public List<Message> Messages { get; set; }
        
        public MessageNotificationViewModel() { }

        public MessageNotificationViewModel(List<Message> messages)
        {
            Messages = messages;

        }

        public string getSenderName(Message message)
        {
            User sender = db.Users.SqlQuery("dbo.User_Select @p0", message.UserID).SingleOrDefault();
            return sender.FirstName + ' ' + sender.LastName;
        }

		public string getProfilePic(Message message) {
			User user = db.Users.SqlQuery("dbo.User_Select @p0", message.UserID).SingleOrDefault();
			Image profilePicture = Image.GetProfileImages(user.UserID, FileType.ProfilePicture);
			string profilePic = (profilePicture.Path != "no-image.png") ? "../Images/" + user.UserID + "/" + profilePicture.Path : profilePicture.Path;
			return profilePic;
		}
    }
}