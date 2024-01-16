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
    [Authorize(Roles ="Managers")]
    public class PropertyManagersController : Controller
    {
        private PropertyRentalDBEntities db = new PropertyRentalDBEntities();

        // GET: PropertyManagers
        [Authorize(Roles = "Owners")]
        public ActionResult Index(string searchString)
        {
            var propertyManagers = db.PropertyManagers.Include(p => p.User);

            if (!string.IsNullOrEmpty(searchString))
            {
                propertyManagers = propertyManagers.Where(p => p.FirstName.Contains(searchString));
            }

            return View(propertyManagers.ToList());
        }

        // GET: PropertyManagers/Details/5
        [Authorize(Roles = "Owners")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyManager propertyManager = db.PropertyManagers.Find(id);
            if (propertyManager == null)
            {
                return HttpNotFound();
            }
            return View(propertyManager);
        }

        // GET: PropertyManagers/Create
        [Authorize(Roles = "Owners")]
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "ID", "UserName");
            return View();
        }

        // POST: PropertyManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Owners")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManagerId,UserId,FirstName,LastName")] PropertyManager propertyManager)
        {
            if (ModelState.IsValid)
            {
                // Add the property manager to the database
                db.PropertyManagers.Add(propertyManager);
                db.SaveChanges();

                // Get the RoleId for Property Manager from RoleMaster table
                int propertyManagerRoleId = db.RoleMasters
                    .Where(role => role.RoleName == "Managers")
                    .Select(role => role.ID)
                    .FirstOrDefault();

                // Check if there is an existing entry in UserRolesMapping for the user
                UserRolesMapping userRoleMapping = db.UserRolesMappings.FirstOrDefault(mapping => mapping.UserID == propertyManager.UserId);

                if (userRoleMapping == null)
                {
                    // Add a new entry in UserRolesMapping for the user associated with Property Manager role
                    userRoleMapping = new UserRolesMapping
                    {
                        UserID =(int) propertyManager.UserId, // Use the UserID from Property Manager
                        RoleID = propertyManagerRoleId
                    };

                    db.UserRolesMappings.Add(userRoleMapping);
                }
                else
                {
                    // If the existing RoleId is not 2, update it to 2
                    if (userRoleMapping.RoleID != 2)
                    {
                        userRoleMapping.RoleID = 2;
                        db.Entry(userRoleMapping).State = EntityState.Modified;
                    }
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "ID", "UserName", propertyManager.UserId);
            return View(propertyManager);
        }

        // GET: PropertyManagers/Edit/5
        [Authorize(Roles = "Owners")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyManager propertyManager = db.PropertyManagers.Find(id);
            if (propertyManager == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "ID", "UserName", propertyManager.UserId);
            return View(propertyManager);
        }

        // POST: PropertyManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Owners")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManagerId,UserId,FirstName,LastName")] PropertyManager propertyManager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(propertyManager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "ID", "UserName", propertyManager.UserId);
            return View(propertyManager);
        }

        // GET: PropertyManagers/Delete/5
        [Authorize(Roles = "Owners")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyManager propertyManager = db.PropertyManagers.Find(id);
            if (propertyManager == null)
            {
                return HttpNotFound();
            }
            return View(propertyManager);
        }

        // POST: PropertyManagers/Delete/5
        [Authorize(Roles = "Owners")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PropertyManager propertyManager = db.PropertyManagers.Find(id);
            db.PropertyManagers.Remove(propertyManager);
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
