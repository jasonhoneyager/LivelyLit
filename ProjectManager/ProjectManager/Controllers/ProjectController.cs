using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManager.Models;

namespace ProjectManager.Controllers
{
    public class ProjectController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Project
        public ActionResult Index()
        {
            var projectModels = db.ProjectModels.Include(p => p.Category).Include(p => p.ProjectStatus);
            return View(projectModels.ToList());
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectModels projectModels = db.ProjectModels.Find(id);
            if (projectModels == null)
            {
                return HttpNotFound();
            }
            return View(projectModels);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            ViewBag.projectCategoryID = new SelectList(db.CategoryModels, "ID", "categoryName");
            ViewBag.projectStatusID = new SelectList(db.ProjectStatusModels, "ID", "projectStatusName");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,projectName,projectCategoryID,projectDescription,projectRequestedDueDate,projectOfferedPaymentType,projectOfferedPaymentAmount,projectPaymentMethod,projectStatusID")] ProjectModels projectModels)
        {
            if (ModelState.IsValid)
            {
                db.ProjectModels.Add(projectModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.projectCategoryID = new SelectList(db.CategoryModels, "ID", "categoryName", projectModels.projectCategoryID);
            ViewBag.projectStatusID = new SelectList(db.ProjectStatusModels, "ID", "projectStatusName", projectModels.projectStatusID);
            return View(projectModels);
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectModels projectModels = db.ProjectModels.Find(id);
            if (projectModels == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectCategoryID = new SelectList(db.CategoryModels, "ID", "categoryName", projectModels.projectCategoryID);
            ViewBag.projectStatusID = new SelectList(db.ProjectStatusModels, "ID", "projectStatusName", projectModels.projectStatusID);
            return View(projectModels);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,projectName,projectCategoryID,projectDescription,projectRequestedDueDate,projectOfferedPaymentType,projectOfferedPaymentAmount,projectPaymentMethod,projectStatusID")] ProjectModels projectModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.projectCategoryID = new SelectList(db.CategoryModels, "ID", "categoryName", projectModels.projectCategoryID);
            ViewBag.projectStatusID = new SelectList(db.ProjectStatusModels, "ID", "projectStatusName", projectModels.projectStatusID);
            return View(projectModels);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectModels projectModels = db.ProjectModels.Find(id);
            if (projectModels == null)
            {
                return HttpNotFound();
            }
            return View(projectModels);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectModels projectModels = db.ProjectModels.Find(id);
            db.ProjectModels.Remove(projectModels);
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
