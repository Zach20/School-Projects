using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bewander.Models;
using System.ComponentModel.DataAnnotations;

namespace Bewander.ViewModels
{
    public class ReviewViewModel
    {
        public string UserID { get; set; }

        // Review Properties
        public int ReviewID { get; set; }
        public string UsersFullName { get; set; }

		public string ProfilePicturePath { get; set; }

        public string ReviewPicturePath { get; set; }

		[Required(ErrorMessage = "Title is required")]
        [StringLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Body is required")]
        [StringLength(700)]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "Selection is required")]
        [Display(Name = "Resident")]
        public ResidentType ResidentType { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [Range(1, 6, ErrorMessage = "Subject is required")]
        [Display(Name = "Subject")]
        public SubjectType SubjectType { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        public int StarRating { get; set; }
        public int CostRating { get; set; }
        public string DatePosted { get; set; }
        public int? ImageID { get; set; }
        public string FileName { get; set; }
        public string Pathname { get; set; }

        // Place Properties

        public string PlaceID { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string PlaceName { get; set; }
        public int? StreetNumber { get; set; }
        public string Route { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Website { get; set; }

        [Display(Name = "Upload Photos")]
        public HttpPostedFileBase Image { get; set; }

        //ImageLike properties
        public string ImageLikeUserID { get; set; }
        public int ImageLikeFileID { get; set; }
        public int ImageLikeID { get; set; }
        public bool ImageLiked { get; set; }
        public int FileID { get; set; }
        public int ImageLikeCount { get; set; }
        public List<bool> ImageLikes { get; set; }


        // Constructors
        public ReviewViewModel() { }

        public ReviewViewModel(Review review, string usersFName, string userLName, Image profilePicture)
        {
            ReviewID = review.ReviewID;
            UserID = review.UserID;
            UsersFullName = usersFName + " " + userLName;
            Title = review.Title;
            Body = review.Body;
            ResidentType = review.ResidentType;
            SubjectType = review.Subject;
            StarRating = review.StarRating;
            CostRating = review.CostRating;
            DatePosted = review.DatePosted.ToLongDateString();
            ImageID = review.ImageID;
			ProfilePicturePath = (profilePicture.Path != "no-image.png") ? review.UserID + "/" + profilePicture.Path : profilePicture.Path;
		}

		public ReviewViewModel(Review review, Image picture, bool imageLikedStatus, int totalLikeCount, Place place
            )
		{
			ReviewPicturePath = review.UserID + "/" + picture.Path;
            ImageID = picture.ID;
            ImageLiked = imageLikedStatus;
            ImageLikeCount = totalLikeCount;
            Title = review.Title;
            PlaceName = place.Name;
        }

        public ReviewViewModel(Review review, PlaceObject place, Image picture)
        {
            ReviewID = review.ReviewID;
            UserID = review.UserID;
            ReviewPicturePath = review.UserID + "/" + picture.Path;
            Title = review.Title;
            Body = review.Body;
            ResidentType = review.ResidentType;
            SubjectType = review.Subject;
            StarRating = review.StarRating;
            CostRating = review.CostRating;
            DatePosted = review.DatePosted.ToLongDateString();
            ImageID = review.ImageID;
            PlaceID = place.Result.PlaceID;
            PlaceName = place.Result.Name;
            foreach (AddressComponent item in place.Result.AddressComponents)
            {
                switch (item.Types[0])
                {
                    case "locality":
                        City = item.LongName;
                        break;
                    case "administrative_area_level_1":
                        State = item.LongName;
                        break;
                    case "country":
                        Country = item.LongName;
                        break;
                    default:
                        break;
                }
            }
        }
        public ReviewViewModel(Review review, PlaceObject place, File image)
        {
            ReviewID = review.ReviewID;
            UserID = review.UserID;
            
            Title = review.Title;
            Body = review.Body;
            ResidentType = review.ResidentType;
            SubjectType = review.Subject;
            StarRating = review.StarRating;
            CostRating = review.CostRating;
            DatePosted = review.DatePosted.ToLongDateString();
            ImageID = review.ImageID;
            FileName = image.FileName;
            Pathname = image.Path;
             
        
            PlaceID = place.Result.PlaceID;
            PlaceName = place.Result.Name;
            foreach (AddressComponent item in place.Result.AddressComponents)
            {
                switch (item.Types[0])
                {
                    case "locality":
                        City = item.LongName;
                        break;
                    case "administrative_area_level_1":
                        State = item.LongName;
                        break;
                    case "country":
                        Country = item.LongName;
                        break;
                    default:
                        break;
                }
            }
        }

    }
}