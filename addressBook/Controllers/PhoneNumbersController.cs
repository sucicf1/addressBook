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
    public class PhoneNumbersController : Controller
    {
        private BookContext db = new BookContext();

        // GET: PhoneNumbers
        //public ActionResult Index()
        //{
        //    var phoneNumbers = db.PhoneNumbers.Include(p => p.User);
        //    return View(phoneNumbers.ToList());
        //}

        // GET: PhoneNumbers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhoneNumber phoneNumber = db.PhoneNumbers.Find(id);
            if (phoneNumber == null)
            {
                return HttpNotFound();
            }
            return View(phoneNumber);
        }

        // GET: PhoneNumbers/Create
        public ActionResult Create(int? UserID)
        {
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "Name");
            SelectList selectList= new SelectList(db.Users, "UserID", "Name");
            if (UserID!=null)
                selectList.Where(s => Int32.Parse(s.Value) == UserID).First().Selected = true;
            ViewBag.UserID = selectList;

            return View();
        }

        // POST: PhoneNumbers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Number,Type,IsDefault,UserID")] PhoneNumber phoneNumber)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    phoneNumber.IsDefault = false;
                    db.PhoneNumbers.Add(phoneNumber);
                    db.SaveChanges();
                    return RedirectToAction("Edit", "Users", new { id = phoneNumber.UserID });
                }
            }
            catch(DataException)
            {
                ModelState.AddModelError("", "Can't save changes. Try later.");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", phoneNumber.UserID);
            return View(phoneNumber);
        }

        public ActionResult MakeDefault(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.PhoneNumbers.Find(id).User;
            try
            {
                IEnumerable<PhoneNumber> phoneNumbers = db.PhoneNumbers.Where(p => p.UserID == user.UserID);
                foreach (var p in phoneNumbers)
                {
                    if (p.PhoneNumberID != id)
                        p.IsDefault = false;
                    else
                        p.IsDefault = true;
                }
                db.SaveChanges();
            }
            catch(DataException)
            {
                ModelState.AddModelError("","Can't save changes. Try later");
            }

            return RedirectToAction("Edit", "Users", new { id = user.UserID });
        }

        // GET: PhoneNumbers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhoneNumber phoneNumber = db.PhoneNumbers.Find(id);
            if (phoneNumber == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", phoneNumber.UserID);
            return View(phoneNumber);
        }

        // POST: PhoneNumbers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhoneNumberID,Number,Type,IsDefault,UserID")] PhoneNumber phoneNumber)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(phoneNumber).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Edit", "Users", new { id = phoneNumber.UserID });
                }
            }
            catch(DataException)
            {
                ModelState.AddModelError("", "Can't save changes. Try later.");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", phoneNumber.UserID);
            return View(phoneNumber);
        }

        // GET: PhoneNumbers/Delete/5
        public ActionResult Delete(int? id,bool? deleteError=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(deleteError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Can't delete. Try again.";
            }
            PhoneNumber phoneNumber = db.PhoneNumbers.Find(id);
            if (phoneNumber == null)
            {
                return HttpNotFound();
            }
            return View(phoneNumber);
        }

        // POST: PhoneNumbers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhoneNumber phoneNumber = db.PhoneNumbers.Find(id);
            try
            {
                db.PhoneNumbers.Remove(phoneNumber);
                db.SaveChanges();
            }
            catch(DataException)
            {
                return RedirectToAction("Delete", new { id = id, deleteError = true });
            }
            return RedirectToAction("Edit","Users",new { id=phoneNumber.UserID });
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
