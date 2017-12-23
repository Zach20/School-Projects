using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bewander.Models
{
    public class ExternalLoginConfirmationViewModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Additional validation to ensure BirthYear is between 1901 and 1999
        [Required]
        [DataType(DataType.Date)]
        [BirthYearRange(min = 1901, max = 2004)] //This should do math to determine the max date eventually.
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        public string Birthdate { get; set; }

        [Display(Name = "Home Town")]
        public string PlaceID { get; set; }

        public string PlaceName { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        ////[Required]
        ////[Display(Name = "Username")]
        ////public string UserName { get; set; }

        //[Required]
        //[Display(Name = "Email")]
        //public string Email { get; set; }
        //public string FirstName { get; internal set; }
        //public string LastName { get; internal set; }
        //public string PlaceID { get; internal set; }
        //public string PlaceName { get; internal set; }
        //public double Lat { get; internal set; }
        //public double Lng { get; internal set; }
        //public DateTime DOB { get; internal set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Keep me logged in.")]
        public bool RememberMe { get; set; }
    }
    // custom validator ensures registrant birth year is within specified range
    public class BirthYearRangeAttribute : ValidationAttribute
    {
        // range values
        public int min { get; set; }
        public int max { get; set; }

        // compares year of associated DateTime to min and max;
        public override bool IsValid(object value)
        {
            DateTime birthdate = (DateTime)value;
            return (birthdate.Year >= min && birthdate.Year <= max);
        }

        // custom error message
        public override string FormatErrorMessage(string name)
        {
            return name + " bewander requires everyone to be at least 13 years old.";
        }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Additional validation to ensure BirthYear is between 1901 and 1999
        [Required]
        [DataType(DataType.Date)]
        [BirthYearRange(min = 1901, max = 2004)] //This should do math to determine the max date eventually.
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        public string Birthdate { get; set; }

        [Display(Name = "Home Town")]
        public string PlaceID { get; set; }

        public string PlaceName { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }
        [Required]
        [MustBeTrue(ErrorMessage ="You must accept the Terms of Service and Privacy Policy!")]
        public bool ToSChecked { get; set; }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value;
        }
    }
    //ooooooook.. so this is going to be an interesting shot at getting both view models into one page..? :D
    public class CombinedLRViewModel
    {
        public RegisterViewModel RegisterViewModel { get; set; }
        public LoginViewModel LoginViewModel { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }

        public bool LockOutEnabled { get; set; }

        public DateTime? LockOutEndDate { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
