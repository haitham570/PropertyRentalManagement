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
    public class AppointmentsController : Controller
    {
        private PropertyRentalDBEntities db = new PropertyRentalDBEntities();

        // GET: Appointments
        [Authorize(Roles = "Owners,Managers,Tenants")]
        public ActionResult Index()
        {
            int currentUserId = GetCurrentUserId();

            var appointments = db.Appointments.Where(a => a.TenantId == currentUserId || a.ManagerId == currentUserId)
                .Include(a => a.PotentialTenant).Include(a => a.PropertyManager).ToList();

            return View(appointments.ToList());
        }

        private int GetCurrentUserId()
        {
            string username = User.Identity.Name;
            User currentUser = db.Users.SingleOrDefault(u => u.UserName == username);
            return currentUser?.ID ?? 0;
        }

        // GET: Appointments/Details/5
        [Authorize(Roles = "Owners,Managers,Tenants")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        [Authorize(Roles = "Owners,Managers,Tenants")]
        public ActionResult Create()
        {
            //Get the current user's ID
            int currentUserId = GetCurrentUserId();

            ViewBag.TenantId = new SelectList(db.PotentialTenants, "TenantId", "FirstName");
            ViewBag.ManagerId = new SelectList(db.PropertyManagers, "ManagerId", "FirstName");

            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(Status)));
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Owners,Managers,Tenants")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentId,TenantId,ManagerId,AppointmentDate,Status")] Appointment appointment, string selectedStatus)
        {
            if (ModelState.IsValid)
            {

                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TenantId = new SelectList(db.PotentialTenants, "TenantId", "FirstName", appointment.TenantId);
            ViewBag.ManagerId = new SelectList(db.PropertyManagers, "ManagerId", "FirstName", appointment.ManagerId);

            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(Status)), selectedStatus);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        [Authorize(Roles = "Owners,Managers,Tenants")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(Status)));


            ViewBag.TenantId = new SelectList(db.PotentialTenants, "TenantId", "FirstName", appointment.TenantId);
            ViewBag.ManagerId = new SelectList(db.PropertyManagers, "ManagerId", "FirstName", appointment.ManagerId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Owners,Managers,Tenants")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentId,TenantId,ManagerId,AppointmentDate,Status")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(Status)));


            ViewBag.TenantId = new SelectList(db.PotentialTenants, "TenantId", "FirstName", appointment.TenantId);
            ViewBag.ManagerId = new SelectList(db.PropertyManagers, "ManagerId", "FirstName", appointment.ManagerId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        [Authorize(Roles = "Owners,Managers,Tenants")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [Authorize(Roles = "Owners,Managers,Tenants")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
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
