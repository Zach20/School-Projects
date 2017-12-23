using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bewander.DAL;
using Bewander.Models;
using System.ComponentModel.DataAnnotations;

namespace Bewander.ViewModels
{
    public class PostInteractionViewModel
    {
        public string UserID { get; set; }
        // Constructors
        public PostInteractionViewModel() { }
        public int PostID { get; set; }
        public string PostUserid { get; set; }
        public Dictionary<string, string> Likes { get; set; }
        public PostInteractionViewModel(User user, int postID, string PostUserID)
        {
            UserID = user.UserID;
            PostID = postID;
            PostUserid = PostUserID;
            Likes = new Dictionary<string, string>();
        }

        


    }
}