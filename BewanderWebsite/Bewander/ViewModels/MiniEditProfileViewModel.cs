using Bewander.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bewander.ViewModels
{
    // Used to modify a user's profile
    public class MiniEditProfileViewModel
    {
        // Property Values
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "About")]
        [DataType(DataType.MultilineText)]
        public string About { get; set; }
        
        public string HomeTownName { get; set; }
        public string HomeTownID { get; set; }
        public double HomeTownLat { get; set; }
        public double HomeTownLng { get; set; }
    
        public string LastTraveledID { get; set; }
        public string LastTraveledName { get; set; }
        public double LastTraveledLat { get; set; }
        public double LastTraveledLng { get; set; }

        [Display(Name = "Date of Birth")]
        public string DOB { get; set; }

        [Display(Name = "Private Profile")]
        public bool PrivateProfile { get; set; }

        [Display(Name = "Upload Photos")]
        public List<HttpPostedFileBase> Images { get; set; }

        public List<PastLocalViewModel> PastLocals = new List<PastLocalViewModel>();
        public List<FavoritePlaceViewModel> FavoritePlaces = new List<FavoritePlaceViewModel>();

        // Default constructor
        public MiniEditProfileViewModel() { }

        // Populate MiniEditProfileViewModel
        public MiniEditProfileViewModel(User user, UserProfile profile, List<Place> places)
        {
            // user information
            FirstName = user.FirstName;
            LastName = user.LastName;
            About = profile.About;
            DOB = profile.DOB.ToShortDateString();
            PrivateProfile = profile.Private;

            // home town information
            HomeTownID = profile.HomeTown;
            HomeTownName = places.Where(a => a.PlaceID == HomeTownID).Select(a => a.Name).FirstOrDefault();

            // last traveled information
            LastTraveledID = profile.LastTraveled;
            LastTraveledName = places.Where(a => a.PlaceID == LastTraveledID).Select(a => a.Name).FirstOrDefault();

            // split string to get individual place Ids
            string[] placeIDs = profile.PastLocal.Split(',');

            // get and set places based on ID
            foreach(string id in placeIDs)
            {
                // get place name
                string place = places.Where(a => a.PlaceID == id).Select(a => a.Name).FirstOrDefault();

                // check if place exists
                if (!String.IsNullOrEmpty(place))
                {
                    PastLocalViewModel pastLocal = new PastLocalViewModel();
                    pastLocal.PastLocalID = id;
                    pastLocal.PastLocalName = place;

                    // add to list of PastLocalViewModels
                    PastLocals.Add(pastLocal);
                }
                else
                {
                    PastLocalViewModel pastLocal = new PastLocalViewModel();
                    pastLocal.PastLocalName = "";

                    PastLocals.Add(pastLocal);
                }

            }

            // Favorite place 
            // split string to get individual place Ids
            string[] favoritePlaceIDs = profile.FavoritePlace.Split(',');

            // get and set places based on ID
            foreach (string id in favoritePlaceIDs)
            {
                // get place name
                string place = places.Where(a => a.PlaceID == id).Select(a => a.Name).FirstOrDefault();

                // check if place exists
                if (!String.IsNullOrEmpty(place))
                {
                    FavoritePlaceViewModel favoritePlace = new FavoritePlaceViewModel();
                    favoritePlace.FavoritePlaceID = id;
                    favoritePlace.FavoritePlaceName = place;

                    // add to list of FavoritePlaceViewModel
                    FavoritePlaces.Add(favoritePlace);
                }
                else
                {
                    FavoritePlaceViewModel favoritePlace = new FavoritePlaceViewModel();
                    favoritePlace.FavoritePlaceName = "";

                    // add to list of FavoritePlaceViewModel
                    FavoritePlaces.Add(favoritePlace);
                }

            }
        }
    }

    // Enumerable PastLocal data information
    public class PastLocalViewModel
    {
        public string PastLocalID { get; set; }
        public string PastLocalName { get; set; }
        public double PastLocalLat { get; set; }
        public double PastLocalLng { get; set; }
    }

    // Enumerable FavoritePlace data information
    public class FavoritePlaceViewModel
    {
        public string FavoritePlaceID { get; set; }
        public string FavoritePlaceName { get; set; }
        public double FavoritePlaceLat { get; set; }
        public double FavoritePlaceLng { get; set; }
    }
}
