using Final_Property_Rental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Final_Property_Rental.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(UserModel model)
        {
            using (PropertyRentalDBEntities context = new PropertyRentalDBEntities())
            {
                bool IsValidUser = context.Users.Any(user => user.UserName.ToLower() ==
                     model.UserName.ToLower() && user.UserPassword == model.UserPassword);
                if (IsValidUser)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "PropertyManagers");
                }
                ModelState.AddModelError("", "invalid Username or Password");
                return View();
            }

        }

        public ActionResult Signup()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Signup(UserModel model)
        {
            using (PropertyRentalDBEntities context = new PropertyRentalDBEntities())
            {
                // Create a new user
                User newUser = new User
                {
                    UserName = model.UserName,
                    UserPassword = model.UserPassword
                    // You might want to add other properties as needed
                };

                // Add the new user to the Users table
                context.Users.Add(newUser);
                context.SaveChanges();

                // Create a new entry in UserRolesMapping for the new user with "Tenant" role
                UserRolesMapping userRoleMapping = new UserRolesMapping
                {
                    UserID = newUser.ID,
                    RoleID = 3
                };

                // Add the new entry to UserRolesMapping table
                context.UserRolesMappings.Add(userRoleMapping);

                // Create a new PotentialTenant
                PotentialTenant newTenant = new PotentialTenant
                {
                    UserId = newUser.ID, // Set the UserId to the ID of the newly created user
                    FirstName = model.FirstName,
                    LastName = model.LastName
                    // You might want to add other properties as needed
                };

                // Add the new PotentialTenant to the PotentialTenants table
                context.PotentialTenants.Add(newTenant);
                context.SaveChanges();

            }
            return RedirectToAction("Login");
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }

}