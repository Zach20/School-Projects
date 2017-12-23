using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Bewander.Models;
using Bewander.DAL;
using Bewander.ViewModels;
using RestSharp;
using RestSharp.Authenticators;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

// TO LOCK OUT UNCONFIRMED USERS, SEE LINE 218
namespace Bewander.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private BewanderContext db = new BewanderContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
            context = new ApplicationDbContext();
        }
        //[HttpGet]
       // [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        //public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        //{
        //    if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
        //    {
        //        ModelState.AddModelError("", "User Id and Code are required");
        //        return BadRequest(ModelState);
        //    }

        //    IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

        //    if (result.Succeeded)
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return GetErrorResult(result);
        //    }
        //}

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
			return View();
			
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(CombinedLRViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Require the user to have a confirmed email before they can log on.
            var user = await UserManager.FindByNameAsync(model.LoginViewModel.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    ViewBag.errorMessage = "You must have a confirmed email to log on.";
                    return View("Error");
                }
                
            }
            var loggedinUser = await UserManager.FindAsync(model.LoginViewModel.Email, model.LoginViewModel.Password);
            if (loggedinUser != null)
            {
                // Now user have entered correct username and password.
                // Time to change the security stamp
                await UserManager.UpdateSecurityStampAsync(loggedinUser.Id);
            }
            // This doesn't count login failures towards account lockout
            // To disable password failures to trigger account lockout, change to shouldLockout: false
            var result = await SignInManager.PasswordSignInAsync(model.LoginViewModel.Email, model.LoginViewModel.Password, model.LoginViewModel.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    //This code will pretty much be untested, but it's intended to prevent an edge-case where the user is locked out
                    //and their time for LockoutEndDate set to 1:37:59, aka our flag time to indicate an admin lock.
                    //This will hopefully present edge cases where the user is locked out of their account at a really specific time (there's only 1 second of the day
                    //where this is possible, but this is aimed to prevent that). If this code doesn't work... oops. There's really no way to test it without implementing
                    //test driven development principals accross this project.
                    
                    var userid = await UserManager.FindByNameAsync(model.LoginViewModel.Email);
                    DateTime tod = new DateTime(1992, 5, 22, 1, 37, 59);

                    if(userid.LockoutEndDateUtc.Value.TimeOfDay == tod.TimeOfDay)
                    {
                        DateTime newtime = userid.LockoutEndDateUtc.Value;
                        await UserManager.SetLockoutEndDateAsync(userid.Id, newtime.Date.AddMinutes(-1));
                    }
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.LoginViewModel.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

       

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin"))
                                            .ToList(), "Name", "Name");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(CombinedLRViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.RegisterViewModel.Email, Email = model.RegisterViewModel.Email};
                var result = await UserManager.CreateAsync(user, model.RegisterViewModel.Password);

                if (result.Succeeded)
                {
                    // Adds person registering to role user
                    IdentityResult roleUser = UserManager.AddToRole(user.Id, "User");

                    //Create new User
                    User newUser = new User(user.Id, model.RegisterViewModel.FirstName, model.RegisterViewModel.LastName);
                    db.Users.Add(newUser);

                    
                    // Check and see if place exists in database///////////////////////
                    Place place = db.Places.Find(model.RegisterViewModel.PlaceID);

                    if (place == null)
                    {
                        Place newPlace = new Place(model.RegisterViewModel.PlaceID, model.RegisterViewModel.PlaceName, model.RegisterViewModel.Lat, model.RegisterViewModel.Lng);
                        db.Places.Add(newPlace);                     
                    }
                    
                    // Create new UserProfile              
                    UserProfile userProfile = new UserProfile { UserID = user.Id, DOB = model.RegisterViewModel.DOB, HomeTown = model.RegisterViewModel.PlaceID};
                    db.UserProfiles.Add(userProfile);

                    db.SaveChanges();

                    //  Comment the following line to prevent log in until the user is confirmed.
                    // await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your Bewander.com account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    // Uncomment to debug locally without sending email-connect to  Views\Shared\Info.cshtml
                    // TempData["ViewBagLink"] = callbackUrl;
                    // require new users to have a confirmed email before they are logged in (authenticated).
                    ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                         + "before you can log in.";
                    

                    return View("Info");


                   //return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            // As of early Feb 2017 the Registration and Login page are combined.
            //Therefore if registration fails, we want to display the "Login" view, not "Register" if it fails.
            return View("Login",model);
        }

        //
        // GET: /Account/ConfirmEmail
        //2/10/2017 - Chris: I'm not sure why this is labeled as a "get" req.
        //I'm 99% sure this is a post...
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            //return View(result.Succeeded ? "ConfirmEmail" : "Error");
            if (result.Succeeded) {
                //Here's code to add a notification to the DB...
                User user = db.Users.Find(userId);
                string message = user.FirstName + " " + user.LastName + " has joined bewander!";
                string data = userId;
                List<User> results = new List<User>();
                results = db.Users.ToList();
                foreach (User User in results)
                {
                    Notifications notification = new Notifications(2, message, User.UserID, data);
                    db.Notifications.Add(notification);
                    db.SaveChanges();
                }
                return RedirectToAction("CreateProfile", "Users");
            }
            else return View("Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                DateTime LockoutEndDateUTC = (DateTime)(user.LockoutEndDateUtc); //need these parentheses because user.LockoutEndDateUtc is stored as nullable, but it will never be null.
                DateTime tod = new DateTime(1992, 5, 22, 1, 37, 59); //make datetime object with time of day set to 1:37:59
                LockoutEndDateUTC = LockoutEndDateUTC.Date.Add(tod.TimeOfDay); //take the Date from LockoutEndDateUTC, and add the Time of Day shown above to it.
                //Unlock account if user reset their password successfully
                if (user.LockoutEnabled && LockoutEndDateUTC.TimeOfDay != LockoutEndDateUTC.TimeOfDay) { //if users LockoutEnabled field = true... && user is not flagged as banned by admin
                    await UserManager.SetLockoutEnabledAsync(user.Id, false); //reset their lockout status to false
                    await UserManager.SetLockoutEndDateAsync(user.Id, DateTime.Now.AddHours(-8)); //subtract 8 hours from current time in UTC to get Pacific time (and ensure they're unlocked)
                } 
                //and continue...
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            
            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    //return PartialView("_PolicyModalPartial");
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Adds person registering to role user
                    IdentityResult roleUser = UserManager.AddToRole(user.Id, "User");

                    //Create new User
                    User newUser = new User(user.Id, model.FirstName, model.LastName);
                    db.Users.Add(newUser);


                    // Check and see if place exists in database///////////////////////
                    Place place = db.Places.Find(model.PlaceID);

                    if (place == null)
                    {
                        Place newPlace = new Place(model.PlaceID, model.PlaceName, model.Lat, model.Lng);
                        db.Places.Add(newPlace);
                    }

                    // Create new UserProfile              
                    UserProfile userProfile = new UserProfile { UserID = user.Id, DOB = model.DOB, HomeTown = model.PlaceID };
                    db.UserProfiles.Add(userProfile);

                    db.SaveChanges();

                    //  Comment the following line to prevent log in until the user is confirmed.
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your Bewander.com account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    // Uncomment to debug locally without sending email-connect to  Views\Shared\Info.cshtml
                    // TempData["ViewBagLink"] = callbackUrl;
                    // require new users to have a confirmed email before they are logged in (authenticated).
                    ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                         + "before you can log in.";

                    return View("Info");



                    //result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    //if (result.Succeeded)
                    //{
                    //    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    //    //return PartialView("_PolicyModalPartial");
                    //    return RedirectToLocal(returnUrl);
                    //}
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        
        
            #endregion


            // Fill DB with Users
            public async void FillDB()
        {
            RegisterViewModel one = new RegisterViewModel { Password = "Pass1!", DOB = DateTime.Now, FirstName = "Adam", LastName = "Smith", PlaceID = "ChIJ7cv00DwsDogRAMDACa2m4K8", PlaceName = "Chicago" };
            RegisterViewModel two = new RegisterViewModel { Password = "Pass1!", DOB = DateTime.Now, FirstName = "Kendra", LastName = "Smith", PlaceID = "ChIJ7cv00DwsDogRAMDACa2m4K8", PlaceName = "Chicago" };
            RegisterViewModel three = new RegisterViewModel { Password = "Pass1!", DOB = DateTime.Now, FirstName = "Josiah", LastName = "Fitch", PlaceID = "ChIJJ3SpfQsLlVQRkYXR9ua5Nhw", PlaceName = "Portland" };
            RegisterViewModel four = new RegisterViewModel { Password = "Pass1!", DOB = DateTime.Now, FirstName = "Dylan", LastName = "Dykes", PlaceID = "ChIJJ3SpfQsLlVQRkYXR9ua5Nhw", PlaceName = "Portland" };
            RegisterViewModel five = new RegisterViewModel { Password = "Pass1!", DOB = DateTime.Now, FirstName = "Tony", LastName = "Thatch", PlaceID = "ChIJOwg_06VPwokRYv534QaPC8g", PlaceName = "New York" };

            RegisterViewModel[] registerModels = new RegisterViewModel[5];

            registerModels[0] = one;
            registerModels[1] = two;
            registerModels[2] = three;
            registerModels[3] = four;
            registerModels[4] = five;

            for (int i = 0; i < 500; i++)
            {
                for (int x = 0; x < registerModels.Length; x++)
                {
                    Guid guid = new Guid();
                    guid = Guid.NewGuid();
                    string tempEmail = String.Format("email{0}@gmail.com", guid);
                    registerModels[x].Email = tempEmail;

                    var model = registerModels[x];

                    if (ModelState.IsValid)
                    {
                        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                        var result = await UserManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            //Create new User
                           
                            //Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

                            //return Created(locationHeader, TheModelFactory.Create(user));

                            // Create new UserProfile

                            // Check and see if place exists in database///////////////////////
                            Place place = db.Places.Find(model.PlaceID);

                            if (place == null)
                            {
                                Place newPlace = new Place(model.PlaceID, model.PlaceName, model.Lng, model.Lng);
                                db.Places.Add(newPlace);

                            }

                            UserProfile userProfile = new UserProfile { UserID = user.Id, DOB = model.DOB, HomeTown = model.PlaceID };
                            db.UserProfiles.Add(userProfile);

                            db.SaveChanges();


                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        }
                        AddErrors(result);
                    }
                }
            }
        }


    }
}

