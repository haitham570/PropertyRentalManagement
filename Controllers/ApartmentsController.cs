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
    public class ApartmentsController : Controller
    {
        private PropertyRentalDBEntities db = new PropertyRentalDBEntities();

        // GET: Apartments
        [Authorize(Roles = "Owners,Managers,Tenants")]
        public ActionResult Index()
        {
            var apartments = db.Apartments.Include(a => a.Building);
            return View(apartments.ToList());
        }

        // GET: Apartments/Details/5
        [Authorize(Roles = "Owners,Managers,Tenants")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // GET: Apartments/Create
        [Authorize(Roles = "Owners,Managers")]
        public ActionResult Create()
        {
            ViewBag.BuildingId = new SelectList(db.Buildings, "BuildingId", "BuildingName");

            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(Status)));

            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Owners,Managers")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApartmentId,BuildingId,Bedrooms,Bathrooms,Status")] Apartment apartment, string selectedStatus)
        {
            if (ModelState.IsValid)
            {
                apartment.Status = selectedStatus.ToString();

                db.Apartments.Add(apartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildingId = new SelectList(db.Buildings, "BuildingId", "BuildingName", apartment.BuildingId);

            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(Status)), selectedStatus);
            return View(apartment);
        }

        // GET: Apartments/Edit/5
        [Authorize(Roles = "Owners,Managers")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }

            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(Status)));


            ViewBag.BuildingId = new SelectList(db.Buildings, "BuildingId", "BuildingName", apartment.BuildingId);
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Owners,Managers")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApartmentId,BuildingId,Bedrooms,Bathrooms,Status")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(Status)));

            ViewBag.BuildingId = new SelectList(db.Buildings, "BuildingId", "BuildingName", apartment.BuildingId);
            return View(apartment);
        }

        // GET: Apartments/Delete/5
        [Authorize(Roles = "Owners,Managers")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [Authorize(Roles = "Owners,Managers")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Apartment apartment = db.Apartments.Find(id);
            db.Apartments.Remove(apartment);
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
