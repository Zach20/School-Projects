using Bewander.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bewander.ViewModels
{
    public class NearbyPeoplePlaces : ViewModelDbContext
    {
        public string PlaceID { get; set; }
        public List<Place> Places { get; set; }
        public List<UserProfile> UserProfiles { get; set; }
        
        public NearbyPeoplePlaces()
        {
        }

        public NearbyPeoplePlaces(string PlaceID, double radius, bool isMiles = true)
        {
            Place place = db.Places.Find(PlaceID);
            this.Places = GetPlaces(place.Lat, place.Lng, radius, isMiles);
            this.UserProfiles = GetUserProfiles();
        }

        public List<Place> GetPlaces(double lat, double lng, double radius, bool measurementType)
        {
            lat = deg2rad(lat);
            lng = deg2rad(lng);
            int earthRadius = measurementType ? 3959 : 6371; //  3959 earth raidus miles : 6371 earth radius kilometers 

            string query = String.Format(
                "Select * From[dbo].[Place] Where acos(sin({0}) * sin(radians(Lat)) + cos({0}) * cos(radians(Lat)) * cos(radians(Lng) - {1})) * {2} < {3}",
                lat,
                lng,
                earthRadius);

            List<Place> places = db.Places.SqlQuery(query).ToList();

            return places;
        }
        
        public List<UserProfile> GetUserProfiles()
        {
            List<UserProfile> results = new List<UserProfile>();
            List<SearchViewModel> searchList = new List<SearchViewModel>();

            foreach (Place place in this.Places)
            {
                List<UserProfile> userProfiles = db.UserProfiles.SqlQuery("SELECT TOP 10 * FROM [dbo].[UserProfile] WHERE HomeTown = @p0", place.PlaceID).ToList();

                foreach (UserProfile userProfile in userProfiles)
                {
                    User user = db.Users.SqlQuery("dbo.User_Select @p0", userProfile.UserID).SingleOrDefault();
                    Image image = Image.GetProfileImages(user.UserID, FileType.ProfilePicture);
                    SearchViewModel result = new SearchViewModel(user.UserID, user.FirstName, user.LastName, place.Name, image.Path);
                    searchList.Add(result);
                }
            }

            return results;
//            return PartialView("_LocalsSearchResults", searchList.OrderBy(i => i.FirstName));
        }
        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

    }
}