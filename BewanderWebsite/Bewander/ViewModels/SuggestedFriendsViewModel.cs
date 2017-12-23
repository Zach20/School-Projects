using Bewander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bewander.ViewModels
{
    public class SuggestedFriendViewModel
    {
        public List<User> Users { get; set; }
        public List<UserProfile> UserProfiles { get; set; }
        public SuggestedFriendViewModel()
        {
            Users = new List<User>();
            UserProfiles = new List<UserProfile>();
        }


    }
}