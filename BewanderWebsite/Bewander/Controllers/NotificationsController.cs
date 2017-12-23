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
    public class NotificationsController : Controller
    {
        private BewanderContext db = new BewanderContext();

        /*
         * Standard new controller mumbo jumbo, except we don't need it since there are no views. This controller more serves as an API.
        // GET: Relationship
        public ActionResult Index()
        {
            return View();
        }
        */
        public ActionResult FlagNotification(int postID)
        {
            Notifications notification = new Notifications();
            notification.MessageID = postID;
            notification.MessageType = 2;
            notification.Content = "New Flagged Post!";
            notification.DateTime = DateTime.Now;
            notification.isread = false;
            notification.data = null;
            notification.UserID = null;

            if (ModelState.IsValid)
            {
                db.Notifications.Add(notification);
                db.SaveChanges();
               
            }
            return RedirectToAction("Index");
        }


        //Called by AJAX to dismiss a new user notification.
        public string DismissNotification(int ID)
        {
            //Change isread flag to true, since they clicked to get to this controller method.
            Notifications notification = db.Notifications.Find(ID);
            notification.isread = true;
            db.SaveChanges();

            //Should return userID associated with the notification message..
            return notification.data;
        }
    }
}