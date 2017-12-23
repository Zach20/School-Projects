using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bewander.ViewModels;
using Bewander.DAL;
using Newtonsoft.Json;
using System.Net;

namespace Bewander.Models
{
    [Table("Place")]
    public class Place
    {
        private string placeid;
        private string name;
        private double lat;
        private double lng;
        private object[] placeID;
        private string placeName;

        [Required]
        public string PlaceID
        {
            get { return placeid; }
            set { placeid = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public double Lat
        {
            get { return lat; }
            set { lat = value; }
        }

        public double Lng
        {
            get { return lng; }
            set { lng = value; }
        }


        public virtual ICollection<Review> Reviews { get; set; }
        //public virtual ICollection<Image> Images { get; set; }


        // Constructors
        public Place()
        {
            Reviews = new List<Review>();
            //Images = new List<Image>();
        }

        public Place(ReviewViewModel model)
        {
            placeid = model.PlaceID;
            name = model.PlaceName;
            lat = model.Lat;
            lng = model.Lng;
            Reviews = new List<Review>();
            //Images = new List<Image>();
        }

        public Place(string placeID, string placeName, double Lat, double Lng)
        {
            placeid = placeID;
            name = placeName;
            lat = Lat;
            lng = Lng;
            Reviews = new List<Review>();
        }

        public Place(object[] placeID, string placeName, double lat, double lng)
        {
            this.placeID = placeID;
            this.placeName = placeName;
            this.lat = lat;
            this.lng = lng;
        }

        // Functions 

    }
}