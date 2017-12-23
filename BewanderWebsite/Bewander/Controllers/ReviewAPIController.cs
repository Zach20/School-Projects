using System;
using Bewander.Models;
using Bewander.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Bewander.DAL;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Web.Hosting;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Web;
using System.Threading;

namespace Bewander.Controllers
{
    public class ReviewAPIController : ApiController
    {
        //need to access bewander db... :)
        private BewanderContext db = new BewanderContext();
        class FileResult : IHttpActionResult
        {
            private readonly string _filePath;
            private readonly string _contentType;

            public FileResult(string filePath, string contentType = null)
            {
                if (filePath == null) throw new ArgumentNullException("filePath");

                _filePath = filePath;
                _contentType = contentType;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(System.IO.File.OpenRead(_filePath))
                };

                var contentType = _contentType ?? MimeMapping.GetMimeMapping(Path.GetExtension(_filePath));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                return Task.FromResult(response);
            }
        }
        public IHttpActionResult Get(string id)
        {
            var newestReview = db.Reviews.OrderByDescending(t => t.DatePosted).FirstOrDefault();
            User user = db.Users.Find(newestReview.UserID);
            if (id == "newestreview")
            {
                string fullname = user.FirstName + " " + user.LastName;
                string webUrl = "http://bewander.com/Reviews/DisplayReviews?PlaceID=" + newestReview.PlaceID;

                var placeinfo = db.Places.Find(newestReview.PlaceID);

                ReviewAPIViewModel review = new ReviewAPIViewModel { PlaceName = placeinfo.Name, Website = webUrl, CostRating = newestReview.CostRating, StarRating = newestReview.StarRating, ReviewID = newestReview.ReviewID, Title = newestReview.Title, Body = newestReview.Body, DatePosted = newestReview.DatePosted, UsersFullName = fullname, ResidentType = (int)newestReview.ResidentType, SubjectType = (int)newestReview.Subject };

                string jsonString = JsonConvert.SerializeObject(review);
                return Ok(jsonString);
            }
            else if(id == "newestreviewphoto")
            {
                Image image = Image.GetProfileImages(user.UserID, FileType.ProfilePicture);

                string fileName = HostingEnvironment.MapPath(@"~\Images\" + newestReview.UserID + @"\" + image.Path);
                
                var fileInfo = new FileInfo(fileName);

                return !fileInfo.Exists ? (IHttpActionResult) NotFound() : new FileResult(fileInfo.FullName);
            }
            else { return Ok("Houston, we have a problem."); }
        }
    }
}