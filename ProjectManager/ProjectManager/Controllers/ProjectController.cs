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
            if (User.IsInRole("Writer"))
            {
                var projectModels = db.ProjectModels.Include(p => p.Category).Include(p => p.ProjectStatus);
                return View(projectModels.ToList());
            }
            else
            {
                var projectModels = db.ProjectModels.Include(p => p.Category).Include(p => p.ProjectStatus).Where(x => x.projectClientID == System.Web.HttpContext.Current.User.Identity.Name);
                return View(projectModels.ToList());
            }
        }

        public ActionResult ProjectCalendar()
        {
            if (User.IsInRole("Writer"))
            {
                var calendarevents = new List<EventViewModel>();
                foreach (ProjectModels project in db.ProjectModels)
                {
                    var events = new EventViewModel();
                    events.title = project.projectName;
                    events.start = project.projectRequestedDueDate.ToShortDateString();
//1   Pending Approval
//2   Declined
//3   Accepted
//4   Work In Progress
//5   Completed, Awaiting Payment
//6   Completed, Payment Received
                    if (project.projectStatusID == 4)
                    {
                        events.color = "orange";
                    }
                    calendarevents.Add(events);
                }
                return View(calendarevents);
            }
            else
            {
                return RedirectToRoute(new
                {
                    Controller = "Project",
                    Action = "Index"
                });
            }
        }

        private IQueryable<ProjectModels> PopulateCalendar()
        {
            var calendarItems = from ProjectModels in db.ProjectModels where ProjectModels.projectStatusID != 1 where ProjectModels.projectStatusID != 2 select ProjectModels;
            return calendarItems;
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
            ViewBag.projectPaymentTypeID = new SelectList(db.PaymentTypeModels, "ID", "projectPaymentType");
            ViewBag.projectPaymentMethodID = new SelectList(db.PaymentMethodModels, "ID", "projectPaymentMethod");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,projectName,projectCategoryID,projectDescription,projectRequestedDueDate,projectPaymentTypeID,projectOfferedPaymentAmount,projectPaymentMethodID")] ProjectModels projectModels)
        {
            if (ModelState.IsValid)
            {
                projectModels.projectClientID = User.Identity.Name;
                projectModels.projectStatusID = 1;
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
            ViewBag.projectPaymentTypeID = new SelectList(db.PaymentTypeModels, "ID", "projectPaymentType", projectModels.projectPaymentTypeID);
            ViewBag.projectPaymentMethodID = new SelectList(db.PaymentMethodModels, "ID", "projectPaymentMethod", projectModels.projectPaymentMethodID);
            return View(projectModels);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,projectName,projectClientID,projectCategoryID,projectDescription,projectRequestedDueDate,projectPaymentTypeID,projectOfferedPaymentAmount,projectPaymentMethodID,projectStatusID")] ProjectModels projectModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.projectCategoryID = new SelectList(db.CategoryModels, "ID", "categoryName", projectModels.projectCategoryID);
            ViewBag.projectStatusID = new SelectList(db.ProjectStatusModels, "ID", "projectStatusName", projectModels.projectStatusID);
            ViewBag.projectPaymentTypeID = new SelectList(db.PaymentTypeModels, "ID", "projectPaymentType", projectModels.projectPaymentTypeID);
            ViewBag.projectPaymentMethodID = new SelectList(db.PaymentMethodModels, "ID", "projectPaymentMethod", projectModels.projectPaymentMethodID);
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
