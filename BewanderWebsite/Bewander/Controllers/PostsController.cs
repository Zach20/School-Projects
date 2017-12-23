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
using Bewander.ViewModels;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace Bewander.Controllers
{
    public class PostsController : Controller
    {
        private BewanderContext db = new BewanderContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        /***************** ACTUAL POSTS ************/
        public PartialViewResult _Newsfeed()
        {
            string userID = User.Identity.GetUserId();
            List<string> friendsList = RelationshipViewModel.GetFriendsID(userID);
            friendsList.Add(userID);
            List<NewsFeedViewModel> postList = NewsFeedViewModel.GetNewsFeed(friendsList);

            return PartialView("_Newsfeed", postList);
        }

        /// <summary>
        /// Creats new post
        /// </summary>
        /// <param name="model"></param>
        /// <param name="postPhoto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost([Bind(Include = "Caption")]CreatePostViewModel createPostViewModel, HttpPostedFileBase postPhoto)
        {
            if (ModelState.IsValid && (createPostViewModel.Caption != null || postPhoto != null))
            {
                createPostViewModel.UserID = User.Identity.GetUserId();
                NewsFeedViewModel formattedPost = Post.CreatePost(createPostViewModel, postPhoto);
                return PartialView("_PostDisplay", formattedPost);
            }
            return (null);
        }

        /// <summary>
        /// Like and Comment section of a NewsFeedViewModel
        /// </summary>
        /// <param name="postID"></param>
        /// <returns></returns>
        public PartialViewResult _PostInteractions(int postID)
        {
            string userID = User.Identity.GetUserId(); // Get current userID to check and see if they've liked the post/comment already
            User user = db.Users.Find(userID);
            Post post = db.Posts.Find(postID);

            PostInteractionViewModel postInteractionViewModel = new PostInteractionViewModel(user, postID, post.UserID);

            foreach (var item in post.Likes)
            {
                User userLike = db.Users.Find(item.UserID);
                try
                {
                    postInteractionViewModel.Likes.Add(userLike.UserID, userLike.FirstName + " " + userLike.LastName); //Add  user if userID not already in db 
                }
                catch (Exception)
                {

                    return PartialView("Post/_PostInteraction", postInteractionViewModel);
                }
            }

            return PartialView("Post/_PostInteraction", postInteractionViewModel);
        }

        /***************** END OF ACTUAL POSTS ************/


        /***************** POST LIKING ************/

        public PartialViewResult LikePost(int postID)
        {
            string userID = User.Identity.GetUserId();
            Post post = db.Posts.Find(postID);
            LikeViewModel likesViewModel = new LikeViewModel();
            bool userAlreadyLiked = false;
            foreach (var item in post.Likes)
            {
                User user = db.Users.Find(item.UserID);
                likesViewModel.Likes.Add(user.UserID, user.FirstName + " " + user.LastName);
                if (item.UserID == userID) // check to see if the current user has already liked this post
                {
                    userAlreadyLiked = true;
                }
            }

            if (userAlreadyLiked == false)
            {
                User currentUser = db.Users.Find(userID);
                Like like = new Like(userID);
                post.Likes.Add(like);
                db.Likes.Add(like);
                likesViewModel.Likes.Add(currentUser.UserID, currentUser.FirstName + " " + currentUser.LastName);
                db.SaveChanges();
            }

            return PartialView("Post/_Like", likesViewModel);
        }

        public PartialViewResult UnlikePost(int postID)
        {
            string userID = User.Identity.GetUserId(); // GET: Current users ID 
            Post post = db.Posts.Find(postID); // GET: The "Post" the user is interacting with
            Like like = post.Likes.Where(i => i.UserID == userID).FirstOrDefault(); // GET: Current users "Like" on post
            post.Likes.Remove(like); // REMOVE: "Like" from the ICollection<Likes> from "Post"
            db.Likes.Remove(like); // REMOVE: "Like" from DB
            db.SaveChanges();

            LikeViewModel likesViewModel = new LikeViewModel(); // Dictionary to be sent to view

            return PartialView("Post/_Like", likesViewModel); // RETURN: Dictionary of "Likes"
        }

        /***************** END OF POST LIKING ************/


        /***************** FLAG POST ************/

        public ActionResult FlagPost(int PostID)
        {
            Post post = db.Posts.Find(PostID); // GET: The "Post" the user flagged
            Flag flag = new Flag(PostID);
            Notifications notification = new Notifications();

            post.Flag = flag.FlagStatus;
            post.ID = flag.PostID;

            notification.MessageType = 3;
            notification.Content = "Has been flagged";

            db.SaveChanges();

            return RedirectToAction("Index", "Users");
        }

        /***************** END OF FLAG POST ************/



        /***************** POST COMMENTS ************/

        /// <summary>
        /// GET all comments for image/caption
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>_Comments Partial</returns>
        public ActionResult GetAllComments(int Id)
        {
            try { 
                Post post = db.Posts.Where(a => a.ID == Id).FirstOrDefault();

                PostCommentsViewModel postCommentsViewModel = GetComments(Id, post);
                return PartialView("Post/_Comment", postCommentsViewModel);
            }
            catch (Exception e)
            {
                PostCommentsViewModel postCommentsViewModel = new PostCommentsViewModel() { Error = e.Message };
                return PartialView("Post/_Comment", postCommentsViewModel);
            }
        }

        /// <summary>
        /// Add a comment for the posted image/caption POST
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="UserComment"></param>
        /// <returns>_Comments Partial</returns>
        [HttpPost]
        public ActionResult AddComment(int Id, string UserComment)
        {
            string userID = User.Identity.GetUserId(); 
            User user = db.Users.Find(userID);
            Post post = db.Posts.Where(a => a.ID == Id).FirstOrDefault();
            try
            {
                if (UserComment.Trim().Length == 0)
                { throw new Exception("Invalid comment submission"); }
                Comment newComment = new Comment()
                {
                    Content = UserComment,
                    Flag = 1,
                    DatePosted = DateTime.Now,
                    Level = 1,
                    PostID = post.ID,
                    Post = post,
                    User = user,
                    Likes = new List<Like>()
                };
                post.Comments.Add(newComment);
                db.Comments.Add(newComment);
                db.SaveChanges();
                PostCommentsViewModel postCommentsViewModel = GetComments(Id, post);
                return PartialView("Post/_Comment", postCommentsViewModel);
            } catch(Exception e)
            {
                PostCommentsViewModel postCommentsViewModel = new PostCommentsViewModel() { Error = e.Message };
                return PartialView("Post/_Comment", postCommentsViewModel);
            }
        }

        /// <summary>
        /// Delete A comment
        /// </summary>
        /// <param name="PostId"></param>
        /// <param name="CommentId"></param>
        /// <returns>Json success or error</returns>
        /// 
        public ActionResult DeleteComment(int PostId, int CommentId)
        {
            string userID = User.Identity.GetUserId();
            Post post = db.Posts.Where(a => a.ID == PostId).FirstOrDefault();
            try
            {
                Comment comment = post.Comments.Where(a => a.ID == CommentId).FirstOrDefault();
                if (comment.User.UserID != userID)
                {
                    return Json(new ResponseViewModel()
                    {
                        Success = false,
                        Error = new ResponseViewModelError() { Message = "Invalid user trying to delte comment", Code = 500 }
                    });
                }
                while (comment.Likes.Count() > 0)
                {
                    var commentLike = comment.Likes.First();
                    comment.Likes.Remove(commentLike);
                    db.Likes.Remove(commentLike);
                }
                post.Comments.Remove(comment);
                db.Comments.Remove(comment);
                db.SaveChanges();
                return Json(new ResponseViewModel() { Success = true });
            }
            catch (Exception e)
            {
                return Json(new ResponseViewModel()
                {
                    Success = false,
                    Error = new ResponseViewModelError() { Message = "Something wrong happend: " + e.Message, Code = 210 }
                });
            }
        }

        /// <summary>
        /// Like A comment
        /// </summary>
        /// <param name="PostId"></param>
        /// <param name="CommentId"></param>
        /// <returns>Json success or error</returns>
        /// 
        public ActionResult LikeComment(int PostId, int CommentId)
        {
            string userID = User.Identity.GetUserId();
            Post post = db.Posts.Where(a => a.ID == PostId).FirstOrDefault();
            Comment comment = post.Comments.Where(a => a.ID == CommentId).FirstOrDefault();       
            try
            {
                Like like = comment.Likes.Where(a => a.UserID == userID).FirstOrDefault();
                if (like == null)
                {
                    Like newLike = new Like(userID);
                    comment.Likes.Add(newLike);
                    db.Likes.Add(newLike);
                    db.SaveChanges();
                    return Json(new LikeResponseViewModel() { Success = true, NumberOfLikes = comment.Likes.Count() });
                } else
                {
                    return Json(new LikeResponseViewModel() {
                        Success = false,
                        Error = new ResponseError() { Message = "User already liked this comment", Code = 310 }
                    });
                }
            } catch (Exception e)
            {
                return Json(new LikeResponseViewModel()
                {
                    Success = false,
                    Error = new ResponseError() { Message = "Something wrong happend: " + e.Message, Code = 210 }
                });
            }
        }

        public ActionResult UnLikeComment(int PostId, int CommentId)
        {
            string userID = User.Identity.GetUserId();
            Post post = db.Posts.Where(a => a.ID == PostId).FirstOrDefault();
            Comment comment = post.Comments.Where(a => a.ID == CommentId).FirstOrDefault();
            try
            {
                Like like = comment.Likes.Where(a => a.UserID == userID).FirstOrDefault();
                comment.Likes.Remove(like);
                db.Likes.Remove(like);
                db.SaveChanges();
                return Json(new LikeResponseViewModel() { Success = true, NumberOfLikes = comment.Likes.Count() });
            }
            catch (Exception e)
            {
                return Json(new LikeResponseViewModel()
                {
                    Success = false,
                    Error = new ResponseError() { Message = "Something wrong happend: " + e.Message, Code = 210 }
                });
            }
        }

        public PostCommentsViewModel GetComments(int Id, Post post = null)
        {
            if (post == null)
            {
                post = db.Posts.Where(a => a.ID == Id).FirstOrDefault();
            }
            try
            {
                PostCommentsViewModel postCommentsViewModel = new PostCommentsViewModel();

                foreach (var comment in post.Comments)
                {
                    Image profilePicture = Image.GetProfileImages(comment.User.UserID, FileType.ProfilePicture);
                    var postComment = new PostComment()
                    {
                        Id = comment.ID,
                        Content = comment.Content,
                        PostId = post.ID,
                        UserId = comment.User.UserID,
                        UserFullName = comment.User.FirstName + " " + comment.User.LastName,
                        UserProfilePic = (profilePicture.Path != "no-image.png") ? comment.User.UserID + "/" + profilePicture.Path : profilePicture.Path,
                        DatePosted = comment.DatePosted,
                        Likes = new List<CommentLike>(),
                        CurrentUserLiked = false,
                        CurrentUserCreatedComment = (comment.User.UserID == User.Identity.GetUserId()) ? true : false
                    };
                    
                    if (comment.Likes != null)
                    {
                        string currentUser = User.Identity.GetUserId();
                        foreach (var like in comment.Likes)
                        {
                            var commentLike = new CommentLike() { Id = like.ID };
                            if (like.UserID != null)
                            {
                                try
                                {
                                    User userLike = db.Users.Find(like.UserID);
                                    commentLike.UserFullName = userLike.FirstName + " " + userLike.LastName;
                                    commentLike.UserId = like.UserID;

                                    if (like.UserID == currentUser)
                                    {
                                        postComment.CurrentUserLiked = true;
                                    }
                                }
                                catch (Exception e) { }
                            }
                            postComment.Likes.Add(commentLike);
                        }
                    } else
                    {
                        comment.Likes = new List<Like>();
                        db.SaveChanges();
                    }
  
                    postCommentsViewModel.Comments.Add(postComment);
                };

                return postCommentsViewModel;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /***************** END OF POST COMMENTS ************/



        /***************** REMOVE POST ************/
        public ActionResult RemovePost(int id)
        {

            Post post = db.Posts.Where(a => a.ID == id).FirstOrDefault();


            if (post != null)
            {
                while (post.Likes.Count() > 0)
                {
                    var postLike = post.Likes.First();
                    post.Likes.Remove(postLike);
                    db.Likes.Remove(postLike);
                }

                while (post.Comments.Count() > 0)
                {
                    var postComment = post.Comments.First();
                    while (postComment.Likes.Count() > 0)
                    {
                        var commentLike = postComment.Likes.First();
                        postComment.Likes.Remove(commentLike);
                        db.Likes.Remove(commentLike);
                    }
                    post.Comments.Remove(postComment);
                    db.Comments.Remove(postComment);
                }
                db.Posts.Remove(post);  //Removing POST from database
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Users");

        }
        /***************** END OF REMOVE POST ************/



        /***************** PUBLIC IMAGES ************/
        public ViewResult PublicImages()
        {
            List<ReviewViewModel> reviewImages = new List<ReviewViewModel>();
            IEnumerable<Review> reviews = db.Reviews.ToList();

            foreach (Review review in reviews.Reverse())
            {
                Image picture = (review.ImageID != null) ? db.Images.Find(review.ImageID) : new Image();

                if (review.ImageID != null && picture != null)
                {
                    Place place = db.Places.Find(review.PlaceID);
                    bool imageLikeToView = PublicImageLikedState(review.ImageID);
                    int totalLikes = db.ImageLike.Count(i => i.FileID == review.ImageID);
                    ReviewViewModel newReview =  new ReviewViewModel(review, picture, imageLikeToView, totalLikes, place);
                    reviewImages.Add(newReview);
                }
            }
            return View(reviewImages.OrderByDescending(i => i.ImageLikeCount));
        }


        //GET: checks ImageLike table to see if current user already "liked" an image from a previous session
        public bool PublicImageLikedState(int? ID) //ID passed from PublicImages() as review.ImageID
        {
            string userID = User.Identity.GetUserId(); // get current user ID
            ImageLike imageLike = db.ImageLike.Where(i => i.FileID == ID && i.UserID == userID).FirstOrDefault();
            File file = db.Files.Find(ID);
            var imageLikedStatus = false;

            if (imageLike != null)
            {
                imageLikedStatus = true;
            }
            else
            {
                imageLikedStatus = false;
            }
            return(imageLikedStatus);
        }
        
        //POST: adds user and fileID to ImageLike table in db to "like" the photo
        [HttpPost]
        public ActionResult LikeImage(int ID) //must use ID so it will map properly to tbl_File ID
        {
            ImageLike imageLike = new ImageLike();
            imageLike.FileID = ID;
            imageLike.UserID = User.Identity.GetUserId();
            db.ImageLike.Add(imageLike);
            db.SaveChanges();


            return RedirectToAction("PublicImages"); 
        }

        //POST: finds record containing user and fileID in ImageLike table and removes that record
        [HttpPost]
        public ActionResult UnlikeImage(int ID)
        {
            string userID = User.Identity.GetUserId(); // get current user ID
            File file = db.Files.Find(ID);
            ImageLike imageLike = db.ImageLike.Where(i => i.FileID == ID && i.UserID == userID).FirstOrDefault();
            db.ImageLike.Remove(imageLike);
            db.SaveChanges();

            return RedirectToAction("PublicImages");
        }

        /***************** END OF IMAGES ************/

    }
}