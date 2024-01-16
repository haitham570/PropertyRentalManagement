using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Final_Property_Rental.Models;

namespace Final_Property_Rental.Controllers
{
    public class MessagesController : Controller
    {
        private PropertyRentalDBEntities db = new PropertyRentalDBEntities();

        // GET: Messages
        [Authorize(Roles = "Owners,Managers,Tenants")]
        public ActionResult Index()
        {
            // Get the current user's ID
            int currentUserId = GetCurrentUserId();

            // Retrieve messages where the current user is either the sender or receiver
            var messages = db.Messages
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .Include(m => m.User)
                .Include(m => m.User1)
                .ToList();

            return View(messages);
        }

        // Helper method to get the ID of the currently logged-in user
        private int GetCurrentUserId()
        {
            string username = User.Identity.Name;
            User currentUser = db.Users.SingleOrDefault(u => u.UserName == username);
            return currentUser?.ID ?? 0;
        }

        // GET: Messages/Details/5
        [Authorize(Roles = "Owners,Managers,Tenants")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        [Authorize(Roles = "Owners,Managers,Tenants")]
        public ActionResult Create()
        {

            //Get the current user's ID
            int currentUserId = GetCurrentUserId();


            ViewBag.SenderId = new SelectList(db.Users, "ID", "UserName", currentUserId);
            ViewBag.ReceiverId = new SelectList(db.Users, "ID", "UserName");

            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Owners,Managers,Tenants")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,ReceiverId,Message1,Timestamp")] Message message)
        {
            if (ModelState.IsValid)
            {
                // Get the current user's ID
                int currentUserId = GetCurrentUserId();

                // Set the SenderId to the ID of the currently logged-in user
                message.SenderId = currentUserId;


                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SenderId = new SelectList(db.Users, "ID", "UserName", message.SenderId);
            ViewBag.ReceiverId = new SelectList(db.Users, "ID", "UserName", message.ReceiverId);
            return View(message);
        }

        // GET: Messages/Edit/5
        [Authorize(Roles = "Owners,Managers,Tenants")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.SenderId = new SelectList(db.Users, "ID", "UserName", message.SenderId);
            ViewBag.ReceiverId = new SelectList(db.Users, "ID", "UserName", message.ReceiverId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Owners,Managers,Tenants")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,SenderId,ReceiverId,Message1,Timestamp")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SenderId = new SelectList(db.Users, "ID", "UserName", message.SenderId);
            ViewBag.ReceiverId = new SelectList(db.Users, "ID", "UserName", message.ReceiverId);
            return View(message);
        }

        // GET: Messages/Delete/5
        [Authorize(Roles = "Owners,Managers,Tenants")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [Authorize(Roles = "Owners,Managers,Tenants")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
