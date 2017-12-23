using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bewander.Models;
using System.ComponentModel.DataAnnotations;

namespace Bewander.ViewModels
{
    public class ReviewAPIViewModel
    {
        // Review Properties
        public int ReviewID { get; set; }
        public string UsersFullName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int ResidentType { get; set; }
        public int SubjectType { get; set; }
        public int StarRating { get; set; }
        public int CostRating { get; set; }
        public DateTime DatePosted { get; set; }
        //Place Properties
        public string PlaceName { get; set; }
        public string Website { get; set; }

        // Constructors
        public ReviewAPIViewModel() { }


        //Need to add a better constructor so that I don't need to do so much manual stuff in ReviewAPIController... But this works for now!
    }
}