using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Bewander.DAL;
using Bewander.Models;
using Bewander.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace Bewander.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext ac = new ApplicationDbContext();
        private static BewanderContext bc = new BewanderContext();

        // GET: Admin
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // Tanvir(23/12/16): Creating Paged List

            ViewBag.CurrentSort = sortOrder;
            var Users = from u in bc.Users
                        select u;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                Users = Users.Where(u => u.LastName.ToLower().Contains(searchString.ToLower()) || u.FirstName.ToLower().Contains(searchString.ToLower()));

            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            // End of PagedList


            // Create ViewModel to run list function
            AdminUserViewModel vm = new AdminUserViewModel();
            // Assemble lists to be passed to list function
            List<AdminUserViewModel> viewModels = new List<AdminUserViewModel>();
            List<ApplicationUser> applicationUsers = ac.Users.ToList();
            List<User> users = Users.ToList();
            List<UserProfile> userProfiles = bc.UserProfiles.ToList();
            List<Place> places = bc.Places.ToList();
            List<Review> reviews = bc.Reviews.ToList();
            List<Post> posts = bc.Posts.ToList();
            List<Post> postFlags = bc.Posts.Where(i => i.Flag == 1).ToList();
            List<Review> reviewFlags = bc.Reviews.Where(i => i.Flag == 1).ToList();
            //pass data to list function
            vm.AdminUserList(viewModels,
             users,
             userProfiles,            
             places,
             applicationUsers,
             posts,
             postFlags,
             reviews,
             reviewFlags
             );
            // for each viewModel, convert Id hash store in Role to name of role
            foreach(AdminUserViewModel viewModel in viewModels)
            {
                viewModel.Role = ac.Roles.Where(a => a.Id == viewModel.Role).Select(a => a.Name).FirstOrDefault();
            }

            return View(viewModels.ToPagedList(pageNumber,pageSize));
            

        }

        // GET: Admin/Details/5
        public ActionResult Details(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // assemble AdminUserViewModel for passed Id
            ApplicationUser appUser = ac.Users.Where(a => a.Id == Id).FirstOrDefault();
            User person = bc.Users.Where(a => a.UserID == Id).FirstOrDefault();
            UserProfile profile = bc.UserProfiles.Where(a => a.UserID == Id).FirstOrDefault();
            string rid = appUser.Roles.Where(a => a.UserId == Id).Select(a => a.RoleId).FirstOrDefault();
            string role = ac.Roles.Where(a => a.Id == rid).Select(a => a.Name).FirstOrDefault();

            string favPlace = (profile.FavoritePlace ?? "BLAH").Split(',')[0];
            string place = bc.Places.Where(a => a.PlaceID == favPlace).Select(a => a.Name).FirstOrDefault() ?? "N/A";
            string homeTown = bc.Places.Where(a => a.PlaceID == profile.HomeTown).Select(a => a.Name).FirstOrDefault();
            int postFlag = bc.Posts.Count(a => a.UserID == Id && a.Flag > 0);
            int reviewFlag = bc.Reviews.Count(a => a.UserID == Id && a.Flag > 0);
            bool lockOutEnabled = appUser.LockoutEnabled;


            AdminUserViewModel vm = new AdminUserViewModel(appUser, person, profile, role, place,homeTown, postFlag, reviewFlag);

            return View(vm);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                ac.Users.Add(applicationUser);
                ac.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // assemble AdminUserViewModel for passed Id
            ApplicationUser appUser = ac.Users.Where(a => a.Id == Id).FirstOrDefault();
            User person = bc.Users.Where(a => a.UserID == Id).FirstOrDefault();
            UserProfile profile = bc.UserProfiles.Where(a => a.UserID == Id).FirstOrDefault();

            // gets RoleId from user data and converts to role name
            string roleId = appUser.Roles.Where(a => a.UserId == Id).Select(a => a.RoleId).FirstOrDefault();
            string roleName = ac.Roles.Where(a => a.Id == roleId).Select(a => a.Name).FirstOrDefault();

            // gets first entry from favorite places - Linq does not like Split
            string favPlace = (profile.FavoritePlace ?? "BLAH").Split(',')[0];
            string place = bc.Places.Where(a => a.PlaceID == favPlace).Select(a => a.Name).FirstOrDefault() ?? "N/A";
            string homeTown = bc.Places.Where(a => a.PlaceID == profile.HomeTown).Select(a => a.Name).FirstOrDefault() ?? "N/A";

            // counts number of flagged submissions by type
            int postFlag = bc.Posts.Count(a => a.UserID == Id && a.Flag > 0);
            int reviewFlag = bc.Reviews.Count(a => a.UserID == Id && a.Flag > 0);

            bool lockOutEnabled = appUser.LockoutEnabled;

            AdminUserViewModel vm = new AdminUserViewModel(appUser, person, profile, roleName, place, homeTown, postFlag, reviewFlag);

            return View(vm);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminUserViewModel vm)
        {                 
            if (ModelState.IsValid)
            {
                // Saves relevant lockout and role data to the database
                ApplicationUser user = ac.Users.Where(a => a.Id == vm.Id).First();

                string rid = user.Roles.Where(a => a.UserId == vm.Id).Select(a => a.RoleId).FirstOrDefault();
                string role = ac.Roles.Where(a => a.Id == rid).Select(a => a.Name).FirstOrDefault() ?? "no role";
                // only remove current role and add new role if admin has changed role 
                if(role != vm.Role)
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ac));
                    var result1 = userManager.RemoveFromRole(vm.Id, role);
                    var result2 = userManager.AddToRole(vm.Id, vm.Role);
                }
                if (vm.LockOutEnabled)
                {
                    //Over ride the time of day user input (hours minutes seconds)
                    //This allows admin bans to only allow one time of day (1:37:59) to be used for admin applied bans
                    //That allows us to implement other features without database migration
                    //To get rid of this code, change LockoutEnabled from a bool (true / false) to an integer, with 0 = no ban, 1 = admin ban, 2 = automatic ban
                    //And then change other code as necessary. 
                    DateTime originaltime = vm.LockOutEndDate; //store the admin selection time
                    DateTime admintime = new DateTime(1992, 5, 22, 1, 37, 59); //2012 , 2 , 4 are irrelevant since .TimeOfDay method is used below. 1, 37, 59 = 1:37:59
                    DateTime modifiedDates = originaltime.Date.Add(admintime.TimeOfDay); //Change the Time Of Day (hours minutes seconds) to 1:37:59.

                    user.LockoutEnabled = vm.LockOutEnabled;
                    user.LockoutEndDateUtc = modifiedDates;
                }
                else
                {
                    user.LockoutEnabled = vm.LockOutEnabled;
                    user.LockoutEndDateUtc = vm.LockOutEndDate.AddHours(1);//removes the custom lockout flag
                }
                

                // store changes in database
                ac.Entry(user).State = EntityState.Modified;
                ac.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        // GET: Admin/FlaggedPosts
        public ActionResult FlaggedPosts(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // Tanvir(23/12/16): Creating Paged List

            ViewBag.CurrentSort = sortOrder;
            var Users = from u in bc.Users
                        select u;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                Users = Users.Where(u => u.LastName.ToLower().Contains(searchString.ToLower()) || u.FirstName.ToLower().Contains(searchString.ToLower()));

            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            // End of PagedList


            // Create ViewModel to run list function
            FlaggedViewModel fp = new FlaggedViewModel();
            // Assemble lists to be passed to list function
            List<FlaggedViewModel> viewModels = new List<FlaggedViewModel>();
            List<ApplicationUser> applicationUsers = ac.Users.ToList();
            List<Post> posts = bc.Posts.ToList();
            //pass data to list function
            fp.FlaggedList(viewModels,
             applicationUsers,
             posts
             );
           

            return View(viewModels.ToPagedList(pageNumber, pageSize));


        }

        // GET: Admin/Delete/5
        /*        public ActionResult Delete(string id)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ApplicationUser applicationUser = ac.Users.Find(id);
                    if (applicationUser == null)
                    {
                        return HttpNotFound();
                    }
                    return View(applicationUser);
                }
        */
        // POST: Admin/Delete/5
        /*        [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public ActionResult DeleteConfirmed(string id)
                {
                    ApplicationUser applicationUser = ac.Users.Find(id);
                    ac.Users.Remove(applicationUser);
                    ac.SaveChanges();
                    return RedirectToAction("Index");
                }
        */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ac.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
