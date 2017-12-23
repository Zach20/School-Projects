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
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.Web.Hosting;


namespace Bewander.Controllers
{
    public class ImagesController : Controller
    {
        private BewanderContext db = new BewanderContext();

        // GET: Images
        public ActionResult Index()
        {
            var files = db.Files.Include(i => i.User);
            return View(files.ToList());
        }

        // GET: Images/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = (Image)db.Files.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // GET: Images/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName");
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FileName,Path,FileType,Caption,DatePosted,UserID,DateProfilePic")] Image image)
        {
            if (ModelState.IsValid)
            {
                db.Files.Add(image);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", image.UserID);
            return View(image);
        }

        // GET: Images/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = (Image)db.Files.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", image.UserID);
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FileName,Path,FileType,Caption,DatePosted,UserID,DateProfilePic")] Image image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", image.UserID);
            return View(image);
        }

        // GET: Images/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Image image = (Image)db.Files.Find(id);

            if (image == null)
                return HttpNotFound();

            return View(image);
        }

       

        // POST: Images/Delete/5  
        public ActionResult DeleteConfirmed(int id)
        {
            string userID = User.Identity.GetUserId();
            try
            {
                Image image = (Image)db.Files.Find(id);
                db.Files.Remove(image);
                Post post = (from p in db.Posts where p.PhotoID == id select p).FirstOrDefault();
                if(post != null) post.PhotoID = null;
                db.SaveChanges();
                string userFolderName = HostingEnvironment.MapPath(@"~\Images\" + userID + @"\" + image.Path);
                System.IO.File.Delete(userFolderName);
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("ProfilePage", "Users", new { userID = userID });
        }

        public ActionResult MakeProfilePicture(int id, string type)
        {
            if (id <= 10009 && id >= 10000 && type == "cover")
            {
               string uid = User.Identity.GetUserId();
               
                //----ncwToDo----
                switch (id)
                {
                    case 10000:
                        @Url.IsLocalUrl("~Images/WallPaperImages/10000.jpg");
                            break;
                        //nope.
                    
                }

                

                return RedirectToAction("ProfilePage", "Users", new { userID = uid });
            }
            else
            {
                Image image = (Image)db.Files.Find(id);
                image.FileType = (type == "profile") ? FileType.ProfilePicture : (type == "cover") ? FileType.CoverPhoto : FileType.AboutPicture;
                image.DateProfilePic = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("ProfilePage", "Users", new { userID = image.UserID });
            }
        }

        public ActionResult CheckoutWallpaper(string filePath, string UserID)
        {
            ICollection<Image> images = db.Images.SqlQuery("dbo.File_Search @p0", UserID).ToList();
            // gets all images in database of Cover Type and Set to Photo Type
            foreach(Image image in images)
            {
                if (image.FileType == FileType.CoverPhoto)
                {
                    Image revertImage = (Image)db.Files.Find(image.ID);
                    revertImage.FileType = FileType.Photo;
                }
                // gets all images in database of Rented Cover Type and deletes from database
                else if (image.FileType == FileType.RentedCoverPhoto)
                {
                    Image deleteImage = (Image)db.Files.Find(image.ID);
                    db.Files.Remove(deleteImage);
                }
            }
            // get image from folder
            Image newImage = new Image();
            newImage.Path = "../WallPaperImages/" + filePath;
            newImage.FileType = FileType.RentedCoverPhoto;
            newImage.UserID = UserID;
            newImage.DatePosted = DateTime.Now;
            newImage.DateProfilePic = DateTime.Now;
            newImage.flagged = false;
            // save image to database, associated with user, set to FileType.RentedCoverPhoto
            db.Files.Add(newImage);
            // return
            db.SaveChanges();

            return RedirectToAction("ProfilePage", "Users", new { userID = UserID});
        }
        //public ActionResult MakeCoverPhoto(int id)
        //{
        //    Image image = (Image)db.Files.Find(id);
        //    image.FileType = FileType.CoverPhoto;
        //    image.DateProfilePic = DateTime.Now;
        //    db.SaveChanges();
        //    return RedirectToAction("ProfilePage", "Users", new { userID = image.UserID });
        //}

        public ActionResult Caption(int id)
        {
            File file = db.Files.Find(id);
            db.Files.Remove(file);
            db.SaveChanges();
            return RedirectToAction("ProfilePage", "Users", new { userID = file.UserID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // Functions

        /// <summary>
        /// Returns profile picture and username for navbar
        /// </summary>
        /// <returns></returns>
        public string NavbarPicture()
        {
            try
            {
                string userID = User.Identity.GetUserId(); // GET: Current userID
                string firstName = db.Users.SqlQuery("dbo.User_Select @p0", userID).Select(i => i.FirstName).FirstOrDefault(); // GET: FirstName of current User
                Image profilePicture = Image.GetProfileImages(userID, FileType.ProfilePicture); // GET: User profile picture
                string path = (profilePicture.FileName != "Default") ?
                    String.Format("<img src='../Images/{0}/{1}' id='nav-picture' alt='{2}' /> {3} ", userID, profilePicture.Path, profilePicture.FileName, firstName) :
                    String.Format("<img src='../Images/{0}' id='nav-picture' alt='{1}' /> {2} ", profilePicture.Path, profilePicture.FileName, firstName);
                return (path);
            }
            catch (Exception)
            {                 
                throw;
            }      
        }

    }
}
