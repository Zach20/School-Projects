using Bewander.DAL;
using Bewander.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(Bewander.Startup))]
namespace Bewander
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            createRolesandUsers();
            //CreateTestUsers();
        }
        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            BewanderContext bc = new BewanderContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup I am creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin role   
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website We will need this for going live.                  

                var user = new ApplicationUser();
                user.UserName = "Bewander@gmail.com";
                user.Email = "Bewander@gmail.com";
                user.EmailConfirmed = true;

                string userPWD = "Pass1234!";

                var chkUser = UserManager.Create(user, userPWD);


                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {

                    // Create Place
                    Place place = new Place("1", "New York", 40.7127837, -74.00594130000002);
                    bc.Places.Add(place);

                    //Create new User
                    User newUser = new User { UserID = user.Id, FirstName = "Bewander", LastName = "Bewander", Permission = 0, DateJoined = DateTime.Now };
                    bc.Users.Add(newUser);

                    // Create new UserProfile              
                    UserProfile userProfile = new UserProfile { UserID = user.Id, DOB = DateTime.Today, HomeTown = place.PlaceID };
                    bc.UserProfiles.Add(userProfile);

                    bc.SaveChanges();

                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }

            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("PowerUser"))
            {
                var role = new IdentityRole();
                role.Name = "PowerUser";
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }

        // For test databases only, not for live site. Will only run if the number of users is less than 100
        protected void CreateTestUsers()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            BewanderContext context = new BewanderContext();

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            // Immediate query of database for number of users
            List<ApplicationUser> users = db.Users.ToList();

            //Number of users wanted for seeding
            int TestUsers = 100;

            // In case someone forgets to remove it on the live site
            if (users.Count < TestUsers)
            {
                // Creates new users
                for (var i = 0; i < TestUsers; i++)
                {
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = String.Format("User{0}@gmail.com", i.ToString());
                    user.Email = String.Format("User{0}@gmail.com", i.ToString());
                    user.EmailConfirmed = true;

                    string userPWD = "Pass1234!";

                    var chkUser = UserManager.Create(user, userPWD);

                    //Create new User
                    User newUser = new User(user.Id, String.Format("FirstName User{0}", i.ToString()), String.Format("LastName User{0}", i.ToString()));
                    context.Users.Add(newUser);

                    // Create new UserProfile              
                    UserProfile userProfile = new UserProfile { UserID = user.Id, DOB = DateTime.Today, HomeTown = "1" };
                    context.UserProfiles.Add(userProfile);

                    //Add default User to Role Admin
                    if (chkUser.Succeeded)
                    {
                        var result1 = UserManager.AddToRole(user.Id, "User");

                    }
                }

                context.SaveChanges();
            }

        }
    }
}
