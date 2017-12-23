using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bewander.DAL;
using Bewander.Models;
using System.ComponentModel.DataAnnotations;

namespace Bewander.ViewModels
{
    public class SearchViewModel
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string PlaceName { get; set; }
        public string PlaceID { get; set; }

        public SearchViewModel() { }

        public SearchViewModel(string userID, string firstName, string lastName, string profilePicture)
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;
            ProfilePicture = (profilePicture != "no-image.png") ? userID + "/" + profilePicture : profilePicture;
            
        }

        public SearchViewModel(string userID, string firstName, string lastName, string placeName, string profilePicture)
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;
            PlaceName = placeName;
            ProfilePicture = (profilePicture != "no-image.png") ? userID + "/" + profilePicture : profilePicture;
        }

        public SearchViewModel(Place place)
        {
            PlaceName = place.Name;
            PlaceID = place.PlaceID;
        }
    }
}