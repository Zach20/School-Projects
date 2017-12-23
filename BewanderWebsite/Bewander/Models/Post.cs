using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bewander.ViewModels;
using Bewander.DAL;
using System.Data.Entity;

namespace Bewander.Models
{
    [Table("Post")]
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Caption { get; set; }

        public int? PhotoID { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DatePosted { get; set; }

        public int Flag { get; set; }

        public string UserID { get; set; }

        public virtual User User { get; set; }

        public Post()
        {
            Likes = new List<Like>();
            Comments = new List<Comment>();
        }


        // Functions

        public static NewsFeedViewModel CreatePost(CreatePostViewModel creatPostViewModel, HttpPostedFileBase postPhoto)
        {
            using(BewanderContext db = new BewanderContext())
            {
                // GET: Models needed for CreatePostViewModel
                string userID = creatPostViewModel.UserID;
                User user = db.Users.Find(userID);
                Image profilePicture = Image.GetProfileImages(userID, FileType.ProfilePicture);

                Post newPost = new Post { DatePosted = DateTime.Now, Caption = creatPostViewModel.Caption, UserID = userID };
                Image postedPhoto = new Image();

                if (postPhoto != null)
                {
                    Image.SavePhotos(user, postPhoto);
                    postedPhoto = (Image)db.Users.Find(userID).Files.Last();
                    newPost.PhotoID = postedPhoto.ID;
                }

                db.Posts.Add(newPost);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                NewsFeedViewModel postDisplayViewModel = new NewsFeedViewModel(user, profilePicture, postedPhoto, newPost);

                return (postDisplayViewModel);
            }
        }



    }
}