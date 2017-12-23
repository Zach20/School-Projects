using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bewander.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "A valid email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "A subject is required")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        //[Required(ErrorMessage = "A message is required")]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}