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
    public class PotentialTenantsController : Controller
    {
        private PropertyRentalDBEntities db = new PropertyRentalDBEntities();

        // GET: PotentialTenants
        [Authorize(Roles ="Owners")]
        public ActionResult Index(string searchString)
        {
            var potentialTenants = db.PotentialTenants.Include(p => p.User);
            if (!string.IsNullOrEmpty(searchString))
            {
                potentialTenants = potentialTenants.Where(p => p.FirstName.Contains(searchString));
            }
            return View(potentialTenants.ToList());
        }

        // GET: PotentialTenants/Details/5
        [Authorize(Roles = "Owners")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PotentialTenant potentialTenant = db.PotentialTenants.Find(id);
            if (potentialTenant == null)
            {
                return HttpNotFound();
            }
            return View(potentialTenant);
        }

        // GET: PotentialTenants/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "ID", "UserName");
            return View();
        }

        // POST: PotentialTenants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenantId,UserId,FirstName,LastName")] PotentialTenant potentialTenant)
        {
            if (ModelState.IsValid)
            {
                db.PotentialTenants.Add(potentialTenant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "ID", "UserName", potentialTenant.UserId);
            return View(potentialTenant);
        }

        // GET: PotentialTenants/Edit/5
        [Authorize(Roles = "Owners")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PotentialTenant potentialTenant = db.PotentialTenants.Find(id);
            if (potentialTenant == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "ID", "UserName", potentialTenant.UserId);
            return View(potentialTenant);
        }

        // POST: PotentialTenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Owners")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TenantId,UserId,FirstName,LastName")] PotentialTenant potentialTenant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(potentialTenant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "ID", "UserName", potentialTenant.UserId);
            return View(potentialTenant);
        }

        // GET: PotentialTenants/Delete/5
        [Authorize(Roles = "Owners")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PotentialTenant potentialTenant = db.PotentialTenants.Find(id);
            if (potentialTenant == null)
            {
                return HttpNotFound();
            }
            return View(potentialTenant);
        }

        // POST: PotentialTenants/Delete/5
        [Authorize(Roles = "Owners")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PotentialTenant potentialTenant = db.PotentialTenants.Find(id);
            db.PotentialTenants.Remove(potentialTenant);
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
