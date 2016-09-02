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
            return View(db.ProjectModels.ToList());
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
            DropDownViewModel ddvm = new DropDownViewModel();
            ddvm.Project = null;
            ddvm.Categories = new SelectList(db.CategoryModels.ToList(), "ID", "categoryName", ddvm);
            return View(ddvm);
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DropDownViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Project.CategoryID = new List<CategoryModels>() { db.CategoryModels.Find(Convert.ToInt32(model.selectedCategory)) };
                db.ProjectModels.Add(model.Project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
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
            return View(projectModels);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,projectName,projectCategory,projectDescription,projectRequestedDueDate,projectOfferedPaymentType,projectOfferedPaymentAmount,projectPaymentMethod")] ProjectModels projectModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
