using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bewander.DAL;
using Bewander.Models;

namespace Bewander.ViewModels
{
    public class CreatePostViewModel
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public bool Files { get; set; }
        public string Caption { get; set; }

        public CreatePostViewModel() { }

        public CreatePostViewModel(User user, string profilePicture)
        {
            UserID = user.UserID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Files = false;
            ProfilePicture = (profilePicture != "no-image.png") ? user.UserID + "/" + profilePicture : profilePicture;
        }



        //private string userID;
        //private string profilePicture;



        //public string UserID { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string ProfilePicture
        //{
        //    get { return profilePicture; }
        //    set { profilePicture = user.UserID + "/" + profilePicture; }
        //}
        //public bool Files { get; set; }
        //public string Caption { get; set; }

        //public CreatePostViewModel() { }

        //public CreatePostViewModel(User user, string profilePicture)
        //{
        //    UserID = user.UserID;
        //    FirstName = user.FirstName;
        //    LastName = user.LastName;
        //    ProfilePicture = user.UserID + "/" + profilePicture;
        //    Files = false;

        //    if (profilePicture == "no-image.png")
        //    {
        //        ProfilePicture = profilePicture;
        //    }
        //}
    }
}