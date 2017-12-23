using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bewander.DAL;
using Bewander.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bewander.ViewModels
{
    public class FlaggedViewModel
    {
        [Required]
        [Display(Name = "Post ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public string UserID { get; set; }

        [Required]
        [Display(Name = "User Email")]
        public string Email { get; set; }

        public FlaggedViewModel() { }

        public FlaggedViewModel(ApplicationUser appUser, Post post)
        {
            ID = post.ID;
            UserID = post.UserID;
            Email = appUser.Email;


        }
        public void FlaggedList(List<FlaggedViewModel> flaggedPosts,
          List<ApplicationUser> netUsers,
          List<Post> posts)
        {

            var flagposts = posts.Where(a => a.Flag == 1);
            foreach (Post post in flagposts)
            {
                FlaggedViewModel fp = new FlaggedViewModel();
                fp.ID = post.ID;
                fp.UserID = post.UserID;
                fp.Email = netUsers.Where(a => a.Id == post.UserID).Select(a => a.Email).FirstOrDefault();

                flaggedPosts.Add(fp);

            }
            
        }
    }
}