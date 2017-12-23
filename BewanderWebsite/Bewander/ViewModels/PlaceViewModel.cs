using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bewander.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace Bewander.ViewModels
{
    public class PlaceViewModel
    {
        // Place Properties
        public string PlaceID { get; set; }
        public string Name { get; set; }
        public string FormattedAddress { get; set; }
        public string StreetNumber { get; set; }
        public string Route { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Website { get; set; }


        // Constructors
        public PlaceViewModel() { }

        public PlaceViewModel(PlaceObject model)
        {
            PlaceID = model.Result.PlaceID;
            Name = model.Result.Name;
            FormattedAddress = model.Result.FormattedAddress;
            Lat = model.Result.Geometry.Location.Lat;
            Lng = model.Result.Geometry.Location.Lng;
            Website = model.Result.Website;

            foreach (AddressComponent item in model.Result.AddressComponents)
            {
                switch (item.Types[0])
                {
                    case "street_number":
                        StreetNumber = item.LongName;
                        break;
                    case "route":
                        Route = item.LongName;
                        break;
                    case "postal_code":
                        PostalCode = item.LongName;
                        break;
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


        // Functions

        public static async Task<PlaceViewModel> GetPlaceObject(string placeID)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //string result = await client.GetStringAsync(String.Format("https://maps.googleapis.com/maps/api/place/details/json?&placeid={0}&key=AIzaSyBI5B2snURiIE8VkeuNYL2Es3ZZf8veRf4", placeID));
                    string result = await client.GetStringAsync(String.Format("https://maps.googleapis.com/maps/api/place/details/json?&placeid={0}&key=AIzaSyAuQaBzbJrduma-UhUFoWNyLWfJFoR3vac", placeID));
                    PlaceObject jsonObject = JsonConvert.DeserializeObject<PlaceObject>(result);
                    PlaceViewModel placeViewModel = new PlaceViewModel(jsonObject);

                    return placeViewModel;
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public static PlaceViewModel GetPlaceObjectt(string placeID)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    //string result = client.DownloadString(String.Format("https://maps.googleapis.com/maps/api/place/details/json?&placeid={0}&key=AIzaSyBI5B2snURiIE8VkeuNYL2Es3ZZf8veRf4", placeID));
                    string result = client.DownloadString(String.Format("https://maps.googleapis.com/maps/api/place/details/json?&placeid={0}&key=AIzaSyAuQaBzbJrduma-UhUFoWNyLWfJFoR3vac", placeID));
                    PlaceObject jsonObject = JsonConvert.DeserializeObject<PlaceObject>(result);

                    PlaceViewModel placeViewModel = new PlaceViewModel(jsonObject);

                    return placeViewModel;
                }
            }
            catch (Exception)
            {

                throw;
            }


        }




    }
}