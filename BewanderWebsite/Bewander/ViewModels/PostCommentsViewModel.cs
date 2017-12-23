using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bewander.ViewModels
{
    public class PostCommentsViewModel
    {
        public List<PostComment> Comments { get; set; }
        public string Error { get; set;  }

        public PostCommentsViewModel()
        {
            Comments = new List<PostComment>();
        }
    }

    public class PostComment
    {
        public string Content { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserProfilePic { get; set; }
        public DateTime DatePosted { get; set;  }
        public int Id { get; set; }
        public List<CommentLike> Likes { get; set; }
        public bool CurrentUserLiked { get; set; }
        public bool CurrentUserCreatedComment { get; set; }

    }

    public class CommentLike
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
    }
}