using Bewander.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Bewander.DAL
{
    public class BewanderContext : DbContext
    {
        public BewanderContext() : base("BewanderContext") { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<ImageLike> ImageLike { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<User>().MapToStoredProcedures();
            modelBuilder.Entity<UserProfile>().MapToStoredProcedures();
        }

        public System.Data.Entity.DbSet<Bewander.ViewModels.AdminUserViewModel> AdminUserViewModels { get; set; }

        
    }

}