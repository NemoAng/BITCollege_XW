﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BITCollege_XW.Data;
using BITCollege_XW.Models;

namespace BITCollege_XW.Controllers
{
    public class StudentCardsController : Controller
    {
        private readonly BITCollege_XWContext db = new BITCollege_XWContext();

        // GET: StudentCards
        public ActionResult Index()
        {
            var studentCards = db.StudentCards.Include(s => s.Student);
            return View(studentCards.ToList());
        }

        // GET: StudentCards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCard studentCard = db.StudentCards.Find(id);
            if (studentCard == null)
            {
                return HttpNotFound();
            }
            return View(studentCard);
        }

        // GET: StudentCards/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName");
            return View();
        }

        // POST: StudentCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentCardId,StudentId,CardNumber")] StudentCard studentCard)
        {
            if (ModelState.IsValid)
            {
                db.StudentCards.Add(studentCard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName", studentCard.StudentId);
            return View(studentCard);
        }

        // GET: StudentCards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCard studentCard = db.StudentCards.Find(id);
            if (studentCard == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName", studentCard.StudentId);
            return View(studentCard);
        }

        // POST: StudentCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentCardId,StudentId,CardNumber")] StudentCard studentCard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentCard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName", studentCard.StudentId);
            return View(studentCard);
        }

        // GET: StudentCards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCard studentCard = db.StudentCards.Find(id);
            if (studentCard == null)
            {
                return HttpNotFound();
            }
            return View(studentCard);
        }

        // POST: StudentCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentCard studentCard = db.StudentCards.Find(id);
            db.StudentCards.Remove(studentCard);
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
