using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.IO;
using Bewander.DAL;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Web.Hosting;

namespace Bewander.Models
{
    public class Image : File
    {
        // Access database
        private static BewanderContext db = new BewanderContext();

        [DataType(DataType.DateTime)]
        public DateTime? DateProfilePic { get; set; }

        public int? ReviewID { get; set; }

        //public string PlaceID { get; set; }
        //public virtual Place Place { get; set; }

        // Constructor
        public Image() { }

        public Image(HttpPostedFileBase image)
        {
            FileName = System.IO.Path.GetFileName(image.FileName);
            Path = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(image.FileName);
            FileType = FileType.Photo;
            DatePosted = DateTime.Now;
        }


        // Functions
        /// <summary>
        /// Saves photo to to file path for specific user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="upload"></param>
        public static void SavePhotos(User user, HttpPostedFileBase upload)
        {
            if (upload.ContentLength > 0)
            {
                Image photo = new Image(upload);
                photo.UserID = user.UserID;
                db.Files.AddOrUpdate(photo);
                db.SaveChanges();

                string folderPath = HostingEnvironment.MapPath("~/Images/" + user.UserID);
                Directory.CreateDirectory(folderPath);
                upload.SaveAs(folderPath + @"\" + photo.Path);
            }
        }

        public static int SaveReviewPhoto (Review review, HttpPostedFileBase upload)
        {
            Image photo = new Image(upload);
            photo.ReviewID = review.ReviewID;
            photo.UserID = review.UserID;
            db.Files.AddOrUpdate(photo);
            db.SaveChanges();

            string folderPath = HostingEnvironment.MapPath("~/Images/" + review.UserID);
            Directory.CreateDirectory(folderPath);
            upload.SaveAs(folderPath + @"\" + photo.Path);

            return photo.ID;
        }

        /// <summary>
        /// Gets the most current photo that has been made into a profile picture or cover photo
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static Image GetProfileImages(string userID, FileType fileType)
        {
            try
            {
                using (BewanderContext db = new BewanderContext())
                {
                    // Parameters for SQL query
                    object[] parameters = new object[]
                    {
                        new SqlParameter("fileType1", (int)fileType),
                        new SqlParameter("fileType2", (fileType == FileType.CoverPhoto) ? (int)FileType.RentedCoverPhoto : (int)fileType),
                        new SqlParameter("userID", userID)
                    };

                    Image image = db.Images.SqlQuery(
                        "SELECT TOP 1 * FROM[dbo].[File] WHERE (FileType = @fileType1 OR FileType = @fileType2) AND UserID = @userID ORDER BY DateProfilePic DESC", 
                        parameters).SingleOrDefault();

                    if (image.Path == null)
                        throw new NullReferenceException();

                    return (image);
                }

            }
            catch (Exception)
            {
                Image image = (fileType == FileType.ProfilePicture || fileType == FileType.AboutPicture) ? new Image { Path = "no-image.png", FileName = "Default" } : new Image { Path = "cover-image.jpg", FileName = "Default" };
                return (image);
            }
        }

    }
}