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
    public class InboxViewModel
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateJoined { get; set; }
        public string ProfilePicturePath { get; set; }
        public string ProfilePictureName { get; set; }
        public string CoverPhotoPath { get; set; }
        public string CoverPhotoName { get; set; }
        public string CurrentUserID { get; set; }
        public RelationshipStatus? RelationshipStatus { get; set; }
        public int RelationshipID { get; set; }
        public bool Private { get; set; }
        public List<Post> Posts { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Relationship> Friends { get; set; }
        public List<Place> Places { get; set; }

        public InboxViewModel() { }

        public InboxViewModel(User user, UserProfile userProfile, Image profilePicture, Image coverPhoto, string currentUserID, RelationshipStatus status, int relationshipID, bool privateProfile, List<Post> posts, List<Review> reviews, List<Relationship> friends, List<Place> places)
        {
            UserID = user.UserID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            DateJoined = user.DateJoined.ToString();
            ProfilePicturePath = (profilePicture.Path != "no-image.png") ? user.UserID + "/" + profilePicture.Path : profilePicture.Path;
            ProfilePictureName = profilePicture.FileName;
            CoverPhotoName = coverPhoto.FileName;
            CoverPhotoPath = (coverPhoto.Path != "cover-image.jpg") ? user.UserID + "/" + coverPhoto.Path : coverPhoto.Path;
            CurrentUserID = currentUserID;
            RelationshipStatus = status;
            RelationshipID = relationshipID;
            Private = privateProfile;
            Posts = posts;
            Reviews = reviews;
            Friends = friends;
            Places = places;
        }

        public InboxViewModel(User user, UserProfile userProfile, Image profilePicture, Image coverPhoto, string currentUserID)
        {
            UserID = user.UserID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            DateJoined = user.DateJoined.ToString();
            ProfilePicturePath = (profilePicture.Path != "no-image.png") ? user.UserID + "/" + profilePicture.Path : profilePicture.Path;

            ProfilePictureName = profilePicture.FileName;

            CoverPhotoName = coverPhoto.FileName;
            CoverPhotoPath = (coverPhoto.Path != "cover-image.jpg") ? user.UserID + "/" + coverPhoto.Path : coverPhoto.Path;
            CurrentUserID = currentUserID;
        }
    }
}