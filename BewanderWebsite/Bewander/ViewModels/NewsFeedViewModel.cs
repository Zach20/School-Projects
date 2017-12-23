using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Web;
using Bewander.DAL;
using Bewander.Models;
using System.ComponentModel.DataAnnotations;

namespace Bewander.ViewModels
{
    public class NewsFeedViewModel
    {
        public int PostID { get; set; }
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Caption { get; set; }
        public string ProfilePicture { get; set; }
        public string DatePosted { get; set; }
        public string PostedPicture { get; set; }
        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }

        public NewsFeedViewModel() { }

        public NewsFeedViewModel(User user, Image profilePicture, Image postedImage, Post post)
        {
            PostID = post.ID;
            UserID = user.UserID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Caption = post.Caption;
            DatePosted = TimeSpan(post.DatePosted);
            Likes = post.Likes.ToList();
            Comments = post.Comments.ToList();
            ProfilePicture = (profilePicture.Path != "no-image.png") ? user.UserID + "/" + profilePicture.Path : profilePicture.Path;
            PostedPicture = (postedImage.Path != null) ? user.UserID + "/" + postedImage.Path : null;            
        }

        public NewsFeedViewModel(Image postedImage, Post post)
        {

            Caption = post.Caption;
            DatePosted = TimeSpan(post.DatePosted);
            Likes = post.Likes.ToList();
            Comments = post.Comments.ToList();         
            PostedPicture = (postedImage.Path != null) ? post.UserID + "/" + postedImage.Path : null;

        }

        /// <summary>
        /// Gets posts from the users friends
        /// </summary>
        /// <param name="friendsID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static List<NewsFeedViewModel> GetNewsFeed(List<string> friendsID)
        {
            using(BewanderContext db = new BewanderContext())
            {
                List<NewsFeedViewModel> posts = new List<NewsFeedViewModel>();
                foreach (string userID in friendsID)
                {
                    User user = db.Users.Find(userID);
                    Image profilePicture = Image.GetProfileImages(userID, FileType.ProfilePicture);
                    ICollection<Post> userPosts = user.Posts;

                    foreach (Post post in userPosts)
                    {
                        Image postedPicture = (post.PhotoID != null) ? db.Images.Find(post.PhotoID) : new Image();
                        NewsFeedViewModel newPost = new NewsFeedViewModel(user, profilePicture, postedPicture, post);
                        posts.Add(newPost);
                    }
                }

                return (posts.OrderByDescending(i => i.Likes.Count).ToList());
            }

        }

        /// <summary>
        /// Returns timespan string from current time
        /// </summary>
        /// <param name="postDate"></param>
        /// <returns></returns>
        public string TimeSpan(DateTime postDate)
        {
            TimeSpan span = DateTime.Now - postDate;

            string datePosted = "Now";
            if (span.Days > 30)
            {
                //return String.Format("{0:MMM d}", postDate); // Jan 1
                return String.Format("{0: MMM d, yyyy}", postDate); // Jan 1 1992
            }
            else if (span.Days > 0)
            {
                return String.Format("{0} {1} ago", span.Days, span.Days == 1 ? "day" : "days"); // 1 day : 2 days
            }
            else if (span.Hours > 0)
            {
                return String.Format("{0} {1} ago", span.Hours, span.Hours == 1 ? "hr" : "hrs"); // 1 hr : 2 hrs
            }
            else if (span.Minutes > 0)
            {
                return String.Format("{0} {1} ago", span.Minutes, span.Minutes == 1 ? "min" : "mins"); // 1 min : 2 mins
            }

            return (datePosted);

        }


                
    }
}