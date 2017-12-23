using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Bewander.ViewModels;

namespace Bewander.Models
{
    [Table("Review")]
    public partial class Review
    {
        private int reviewid;
        private string userid;
        private string placeid;
        private string title;
        private string body;
        private SubjectType subject;
        private DateTime dateposted;
        private ResidentType residenttype;
        private int starrating;
        private int costrating;
        private int? flag;
        private int? imageid;



        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewID
        {
            get { return reviewid; }
            set { reviewid = value; }
        }

        [Required]
        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }
        public virtual User User { get; set; }

        [Required]
        public string PlaceID
        {
            get { return placeid; }
            set { placeid = value; }
        }
        public virtual Place Place { get; set; }

        public int? ImageID
        {
            get { return imageid; }
            set { imageid = value; }
        }

        [Required]
        [StringLength(50)]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        [Required]
        [StringLength(700)]
        [DataType(DataType.MultilineText)]
        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        [Required]
        [Display(Name = "Subject")]
        public SubjectType Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Posted")]
        public DateTime DatePosted
        {
            get { return dateposted; }
            set { dateposted = value; }
        }

        [Required]
        [Range(0, 2)]
        [Display(Name = "Resident")]
        public ResidentType ResidentType
        {
            get { return residenttype; }
            set { residenttype = value; }
        }

        [Required]
        [Range(0, 10)]
        [Display(Name = "Star Rating")]
        public int StarRating
        {
            get { return starrating; }
            set { starrating = value; }
        }

        [Required]
        [Range(0, 5)]
        [Display(Name = "Cost Rating")]
        public int CostRating
        {
            get { return costrating; }
            set { costrating = value; }
        }

        public int? Flag
        {
            get { return flag; }
            set { flag = value; }
        }


        // Constructors
        public Review() { }

        public Review(string userID, ReviewViewModel model)
        {
            userid = userID;
            placeid = model.PlaceID;
            //cityid = model.City;
            //stateid = model.State;
            //countryid = model.Country;
            title = model.Title;
            body = model.Body;
            subject = model.SubjectType;
            dateposted = DateTime.Now;
            residenttype = model.ResidentType;
            starrating = model.StarRating;
            costrating = model.CostRating;
            imageid = model.ImageID;

        }

    }
}