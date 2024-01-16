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
    public class RoleMastersController : Controller
    {
        private PropertyRentalDBEntities db = new PropertyRentalDBEntities();

        // GET: RoleMasters
        public ActionResult Index()
        {
            return View(db.RoleMasters.ToList());
        }

        // GET: RoleMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleMaster roleMaster = db.RoleMasters.Find(id);
            if (roleMaster == null)
            {
                return HttpNotFound();
            }
            return View(roleMaster);
        }

        // GET: RoleMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RoleName")] RoleMaster roleMaster)
        {
            if (ModelState.IsValid)
            {
                db.RoleMasters.Add(roleMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roleMaster);
        }

        // GET: RoleMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleMaster roleMaster = db.RoleMasters.Find(id);
            if (roleMaster == null)
            {
                return HttpNotFound();
            }
            return View(roleMaster);
        }

        // POST: RoleMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RoleName")] RoleMaster roleMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roleMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roleMaster);
        }

        // GET: RoleMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleMaster roleMaster = db.RoleMasters.Find(id);
            if (roleMaster == null)
            {
                return HttpNotFound();
            }
            return View(roleMaster);
        }

        // POST: RoleMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoleMaster roleMaster = db.RoleMasters.Find(id);
            db.RoleMasters.Remove(roleMaster);
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
