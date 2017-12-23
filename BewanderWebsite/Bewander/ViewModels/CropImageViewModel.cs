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
    public class CropImageViewModel
    {
        public string UserID { get; set; }
        public string ProfilePicturePath { get; set; }
        public string CurrentUserID { get; set; }
        public Image ImageToCrop { get; set; }

        public CropImageViewModel() { }

        public CropImageViewModel(User user, Image profilePicture, string currentUserID, Image imageToCrop)
        {
            UserID = user.UserID;
            ProfilePicturePath = (profilePicture.Path != "no-image.png") ? user.UserID + "/" + profilePicture.Path : profilePicture.Path;
            CurrentUserID = currentUserID;
            ImageToCrop = imageToCrop;
        }
        
    }

}