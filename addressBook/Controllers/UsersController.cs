using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using addressBook.DAL;
using addressBook.Models;

namespace addressBook.Controllers
{
    public class UsersController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Users
        public ActionResult Index(string findInName,string findInSurname,string findInPhoneNumber)
        {
            var users = new List<User>();
            if (!String.IsNullOrEmpty(findInPhoneNumber))
            {
                ViewBag.isSearchedByNumber = true;
                var searchedNumber = findInPhoneNumber.ToString();
                var phoneNumbersMatch = db.PhoneNumbers.Where(
                                            p => p.Number.ToString().Contains(searchedNumber)).ToList();

                users.Clear();
                foreach (var p in phoneNumbersMatch)
                {
                    var user = new User();

                    user.UserID = p.UserID; user.Name = p.User.Name;
                    user.Surname = p.User.Surname; user.BirthDay = p.User.BirthDay;
                    user.Address = p.User.Address; user.Email = p.User.Email;
                    user.PhoneNumbers = new List<PhoneNumber>();

                    user.PhoneNumbers.Add(p);
                    users.Add(user);
                }
            }
            else
            {
                users = db.Users.ToList();
                ViewBag.isSearchedByNumber = false;

                if (!String.IsNullOrEmpty(findInName))
                    users = users.Where(u => u.Name.IndexOf(findInName,StringComparison.OrdinalIgnoreCase)>=0).ToList();
                if (!String.IsNullOrEmpty(findInSurname))
                    users = users.Where(u => u.Surname.IndexOf(findInSurname,StringComparison.OrdinalIgnoreCase)>=0).ToList();

                foreach (var user in users)
                {
                    var phoneNumber = new PhoneNumber();
                    phoneNumber = user.PhoneNumbers.Where(p => p.IsDefault).SingleOrDefault();
                    user.PhoneNumbers.Clear();
                    user.PhoneNumbers.Add(phoneNumber);
                }
            }
            return View(users);
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Surname,BirthDay,Address,Email")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(DataException)
            {
                ModelState.AddModelError("", "Can't save changes.Try later.");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Name,Surname,BirthDay,Address,Email")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(DataException)
            {
                ModelState.AddModelError("", "Can't save changes. Try later.");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id, bool? deleteError=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(deleteError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Can't delete.Try again";
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                User user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
            }
            catch(DataException)
            {
                return RedirectToAction("Delete", new { id = id, deleteError = true });
            }
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
