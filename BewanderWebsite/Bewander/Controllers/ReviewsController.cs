using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bewander.DAL;
using Bewander.Models;
using Bewander.ViewModels;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bewander.Controllers
{
    public class ReviewsController : Controller
    {
        private BewanderContext db = new BewanderContext();

        // GET: Reviews
        [Authorize]
        public ActionResult Index()
        {
            //var reviews = db.Reviews.Include(r => r.User);
            //return View(reviews.ToList());
            return View();
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Title,Body,SubjectType,ResidentType,StarRating,CostRating,PlaceID,PlaceName,Image")] ReviewViewModel model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            string userID = User.Identity.GetUserId();
            if (ModelState.IsValid && userID != null)
            {

                User user = db.Users.SqlQuery("dbo.User_Select @p0", userID).SingleOrDefault();
                PlaceViewModel Placeinfo = PlaceViewModel.GetPlaceObjectt(model.PlaceID);
                Place place = db.Places.SqlQuery("dbo.Place_Select @p0", model.PlaceID).SingleOrDefault();

                if (place == null)
                {
                    db.Database.ExecuteSqlCommand("dbo.Place_Insert @p0, @p1, @p2, @p3", Placeinfo.PlaceID, Placeinfo.Name, Placeinfo.Lat, Placeinfo.Lng);
                    db.SaveChanges();
                }
                /* else if (place.Lng == 0)
                 {
                     db.Database.ExecuteSqlCommand("dbo.Place_Insert @p0, @p1 WHERE place.PlaceID= @p2", Placeinfo.Lat, Placeinfo.Lng, Placeinfo.PlaceID);
                     db.SaveChanges();
                 }*/
                Review review = new Review(userID, model);

                // Save review image if one was uploaded
                if (model.Image != null)
                {
                    int imageID = Image.SaveReviewPhoto(review, model.Image);
                    review.ImageID = imageID;
                }

                db.Database.ExecuteSqlCommand("dbo.Review_Insert @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10",
                                    review.PlaceID, review.UserID, review.Title, review.Body, review.Subject,
                                    review.DatePosted, review.ResidentType, review.StarRating, review.CostRating, review.Flag, review.ImageID);
                db.SaveChanges();
                if (review.ImageID != null)
                {
                    Review revID = db.Reviews.Where(i => i.ImageID == review.ImageID).SingleOrDefault();
                    string query = String.Format("Update [dbo].[File] SET ReviewID = {0} Where ID = {1}", revID.ReviewID, review.ImageID);
                    db.Database.ExecuteSqlCommand(query);
                    db.SaveChanges();
                }
                using (WebClient wc = new WebClient())
                {

                    string url = String.Format("https://maps.googleapis.com/maps/api/place/details/json?&placeid={0}&key=AIzaSyAuQaBzbJrduma-UhUFoWNyLWfJFoR3vac", model.PlaceID);
                    var json = wc.DownloadString(url);
                    PlaceObject jsonObject = JsonConvert.DeserializeObject<PlaceObject>(json);

                    File image = db.Files.Find(review.ImageID);

                    if (review.ImageID != null)


                    {
                        ReviewViewModel formattedReview = new ReviewViewModel(review, jsonObject, image);
                        List<ReviewViewModel> formattedReviewList = new List<ReviewViewModel>();
                        formattedReviewList.Add(formattedReview);
                        return View("_ConfirmReview", formattedReviewList.OrderByDescending(i => i.ReviewID));
                    }
                    else
                    {
                        File image2 = Image.GetProfileImages(userID, FileType.ProfilePicture);
                        if (image2.FileName == "Default")
                        {
                            //or you can use
                            if (image2.Path == "no-image.png")
                            {

                            }
                        }

                        ReviewViewModel formattedReview = new ReviewViewModel(review, jsonObject, image2);
                        List<ReviewViewModel> formattedReviewList = new List<ReviewViewModel>();
                        formattedReviewList.Add(formattedReview);
                        return View("_ConfirmReview", formattedReviewList.OrderByDescending(i => i.ReviewID));
                    }




                }


            }
            return View();
        }


        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", review.UserID);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReviewID,PlaceID,UserID,Title,Body,Subject,DatePosted,ResidentType,StarRating,CostRating,Flag")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", review.UserID);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // -- DISPLAY --
        [HttpPost]
        public ActionResult Display(FormCollection formData)
        {
            // SET: Empty variables for form input values
            string googleID = "";

            // GET: Values from form input
            foreach (var key in formData.AllKeys)
            {
                var value = formData[key];

                if (key.Contains("PlaceID"))
                {
                    googleID = value;
                }
            }

            // NULL googleID: 404 error
            if (googleID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // GET: All reviews with matching GoogleID
            ICollection<Review> reviewsList = db.Places.Find(googleID).Reviews;
            //List<Review> allReviews = AllReviews(googleID);

            //  NULL Review count: Redirect to empty page (Would you like to make a review page)
            //if (allReviews.Count == 0)
            //{
            //    return RedirectToAction("Create");
            //}

            // FORMAT: Set values for viewmodel
            //List<DisplayReviews> reviews = FormatReviews(allReviews);

            List<ReviewViewModel> reviews = new List<ReviewViewModel>();

            // POST: Send values from allReviews to page for display
            return View(reviews);
        }

        public ActionResult DisplayReviews(string PlaceID)
        {
            Place place = db.Places.Find(PlaceID);
            if (place == null)
            {
                Place notFound = new Place("", "No Info On This Place", 0, 0);
                return View(notFound);
            }
            return View(place);
        }

        public PartialViewResult _Summary(string PlaceID, string selected, string userType)
        {
            List<Review> reviews = db.Reviews.Where(i => i.PlaceID == PlaceID).ToList();
            List<ReviewViewModel> formattedReviews = new List<ReviewViewModel>();
            


            foreach (Review review in reviews)
            {
                User user = db.Users.Find(review.UserID);
				Image profilePicture = Image.GetProfileImages(review.UserID, FileType.ProfilePicture);
				ReviewViewModel reviewViewModel = new ReviewViewModel(review, user.FirstName, user.LastName, profilePicture);
                string rtype = review.ResidentType.ToString().ToLower();
                if (rtype == userType || userType == "both")
                {
                    formattedReviews.Add(reviewViewModel);
                }
                return PartialView("DisplayReviews/_Summary", userType);
            }

            switch (selected)
            {
                case "old":
                    List<ReviewViewModel> properlyformattedReviews = formattedReviews.OrderByDescending(i => i.DatePosted).ToList();
                    return PartialView("DisplayReviews/_Summary", properlyformattedReviews);
                case "best":
                    return PartialView("DisplayReviews/_Summary", formattedReviews.OrderByDescending(i => i.StarRating));
                case "worst":
                    return PartialView("DisplayReviews/_Summary", formattedReviews.OrderBy(i => i.StarRating));
                case "new":
                default:
                    List<ReviewViewModel> correctlyformattedReviews = formattedReviews.OrderBy(i => i.DatePosted).ToList();
                    return PartialView("DisplayReviews/_Summary", correctlyformattedReviews);
            }

        }

        public PartialViewResult _Photos(string PlaceID)
        {
            List<Review> reviews = db.Reviews.Where(i => i.PlaceID == PlaceID).ToList();
            List<String> imagePaths = new List<String>();

            foreach (Review review in reviews)
            {
                if (review.ImageID != null)
                {
                    File image = db.Files.Find(review.ImageID);
                    String path = "../../Images/" + image.UserID + "/" + image.Path;
                    imagePaths.Add(path);
                }
            }

            return PartialView("DisplayReviews/_Photos", imagePaths.ToList());
        }

        public PartialViewResult PlaceDetails(string placeID)
        {
            using (WebClient wc = new WebClient())
            {
                // string url = String.Format("https://maps.googleapis.com/maps/api/place/details/json?&placeid={0}&key=AIzaSyBI5B2snURiIE8VkeuNYL2Es3ZZf8veRf4", placeID);
                string url = String.Format("https://maps.googleapis.com/maps/api/place/details/json?&placeid={0}&key=AIzaSyAuQaBzbJrduma-UhUFoWNyLWfJFoR3vac", placeID);
                var json = wc.DownloadString(url);
                PlaceObject jsonObject = JsonConvert.DeserializeObject<PlaceObject>(json);

                PlaceViewModel placeViewModel = new PlaceViewModel(jsonObject);
                return PartialView("DisplayReviews/PlaceDetails", placeViewModel);
            }
        }

        public PartialViewResult GetLocalUsers(double lat, double lng, string radius, bool measurementType)
        {
            List<Place> places = PlaceRadius(lat, lng, Double.Parse(radius), measurementType);
            List<UserProfile> results = new List<UserProfile>();
            List<SearchViewModel> searchList = new List<SearchViewModel>();
            string currentUser = User.Identity.GetUserId();
            foreach (Place place in places)
            {
                List<UserProfile> userProfiles = db.UserProfiles.SqlQuery("SELECT TOP 10 * FROM [dbo].[UserProfile] WHERE HomeTown = @p0", place.PlaceID).ToList();
                string wildPlaceId = "%" + place.PlaceID + "%";
                List<UserProfile> pastProfiles = db.UserProfiles.SqlQuery("SELECT TOP 10 * FROM [dbo].[UserProfile] WHERE PastLocal LIKE @p0", wildPlaceId).ToList();
                foreach (UserProfile pastProfile in pastProfiles)
                {
                    if (pastProfile.UserID != currentUser) userProfiles.Add(pastProfile); //removing the if statement here will show the current user in the locals list
                }
                foreach (UserProfile userProfile in userProfiles)
                {
                    User user = db.Users.SqlQuery("dbo.User_Select @p0", userProfile.UserID).SingleOrDefault();
                    Image image = Image.GetProfileImages(user.UserID, FileType.ProfilePicture);
                    SearchViewModel result = new SearchViewModel(user.UserID, user.FirstName, user.LastName, place.Name, image.Path);
                    if (result.UserID != currentUser) searchList.Add(result); //removing the if statement here will show the current user in the locals list
                }
            }

            return PartialView("_LocalsSearchResults", searchList.OrderBy(i => i.FirstName));
        }

        public List<Place> PlaceRadius(double lat, double lng, double radius, bool measurementType)
        {
            lat = deg2rad(lat);
            lng = deg2rad(lng);
            int earthRadius = (measurementType == true) ? 3959 : 6371; // 6371 earth radius kilometers : 3959 earth raidus miles 

            string query = String.Format("Select * From[dbo].[Place] Where acos(sin({0}) * sin(radians(Lat)) + cos({0}) * cos(radians(Lat)) * cos(radians(Lng) - {1})) * {2} < {3}", lat, lng, earthRadius, radius);

            List<Place> places = db.Places.SqlQuery(query).ToList();

            return places;
        }

        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }








    }
}
