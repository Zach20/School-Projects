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
using System.IO;
using Bewander.ViewModels;

namespace Bewander.Controllers
{
    public class RelationshipsController : Controller
    {
        private BewanderContext db = new BewanderContext();

        // GET: Relationship
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


        // Functions
        ////////////////////////////////////////////////////////

        public void AddFriend(string targetUserID)
        {
            string userID = User.Identity.GetUserId();
            Relationship checkRelationship = db.Relationships.SqlQuery("dbo.Check_Friends @p0, @p1", userID, targetUserID).SingleOrDefault();
            if (checkRelationship == null)
            {
                Relationship relationship = new Relationship(userID, targetUserID, RelationshipStatus.Pending);
                db.Relationships.Add(relationship);
                db.SaveChanges();
            } else if (checkRelationship.Status == RelationshipStatus.NoRelationship) {
                checkRelationship.Status = RelationshipStatus.Pending;
                db.SaveChanges();
            }
        }

        public void ConfirmFriend(int ID)
        {
            Relationship relationship = db.Relationships.Find(ID);
            relationship.Status = RelationshipStatus.Friends;
            db.SaveChanges();
        }

        public void DenyFriend(int ID)
        {
            Relationship relationship = db.Relationships.Find(ID);
            relationship.Status = RelationshipStatus.Denied;
            db.SaveChanges();
        }

        public void BlockUser(string targetUserID)
        {
            string userID = User.Identity.GetUserId();
            Relationship relationship = new Relationship(userID, targetUserID, RelationshipStatus.Blocked);
            db.Relationships.Add(relationship);
            db.SaveChanges();
        }

    }
}