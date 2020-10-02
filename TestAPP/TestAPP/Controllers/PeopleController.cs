using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TestAPP.Models;

namespace TestAPP.Controllers
{
    public class PeopleController : Controller
    {
        private PhoneBookDbEntities db = new PhoneBookDbEntities();

        // GET: People
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                //To get person's added by relevent user
                //get user id
                string user = User.Identity.GetUserId();
                ViewBag.FirstName = user;
                var people = db.People.Where(t => t.AddedBy == user).ToList();
                //var people = db.People.ToList();
                return View(people);
            } catch {
                return View();
            }
        }
        // For Dsahboard
        [Authorize]
        public ActionResult Dashboard()
        {
            try
            {
                List<Person> list = new List<Person>();
                string user = User.Identity.GetUserId();
                var persons = db.People.Where(p => p.AddedBy == user).ToList();
                foreach (var per in persons)
                {
                    list.Add(per);
                }
                ViewBag.message = "yes";
                return View(list);
            } catch
            {
                ViewBag.message = "No list Found";
                return View();
            }
        }
        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            var toGetDetails = db.People.Single(i => i.PersonId == id);
            return View(toGetDetails);
        }

        // GET: People/Create
        [Authorize]
        public ActionResult Create()
        {
            //ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }
        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Person/Create
        [HttpPost]
        public ActionResult Create(Person obj)
        {
            // TODO: Add insert logic here

            // to get user identity 
            String user = User.Identity.GetUserId();

            Person p = new Person();
            //set this new person from the data entered in the form

            p.FirstName = obj.FirstName;
            p.MiddleName = obj.MiddleName;
            p.LastName = obj.LastName;
            p.DateOfBirth = obj.DateOfBirth;
            p.AddedOn = DateTime.Now;
            p.AddedBy = user;
            p.HomeAddress = obj.HomeAddress;
            p.HomeCity = obj.HomeCity;
            p.FaceBookAccountId = obj.FaceBookAccountId;
            p.LinkedInId = obj.LinkedInId;
            p.TwitterId = obj.TwitterId;
            p.EmailId = obj.EmailId;

            // add it into the database
            db.People.Add(p);
            //save update database
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email", person.AddedBy);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonId,FirstName,MiddleName,LastName,DateOfBirth,AddedOn,HomeAddress,HomeCity,FaceBookAccountId,LinkedInId,UpdateOn,TwitterId,EmailId")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email", person.AddedBy);
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
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
