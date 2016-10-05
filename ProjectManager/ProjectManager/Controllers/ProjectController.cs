using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ProjectManager.Models;
using Microsoft.AspNet.Identity;

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
                var calendarevents = PopulateCalendar();
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

        private List<EventViewModel> PopulateCalendar()
        {
            var calendarevents = new List<EventViewModel>();
            foreach (ProjectModels project in db.ProjectModels)
            {
                var events = new EventViewModel();
                events.title = project.projectName;
                events.start = project.projectRequestedDueDate.ToShortDateString();
                events.url = "/project/edit/"+project.ID;
                //1   Pending Approval
                //2   Declined
                //3   Accepted
                //4   Work In Progress
                //5   Completed, Awaiting Payment
                //6   Completed, Payment Received
                if (project.projectStatusID == 1)
                {
                    events.color = "red";
                }
                if (project.projectStatusID == 2)
                {
                    events.color = "grey";
                }
                if (project.projectStatusID == 3)
                {
                    events.color = "blue";
                }
                if (project.projectStatusID == 4)
                {
                    events.color = "purple";
                }
                if (project.projectStatusID == 5)
                {
                    events.color = "yellow";
                }
                if (project.projectStatusID == 6)
                {
                    events.color = "green";
                }

                calendarevents.Add(events);
            }
            return calendarevents;
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
            ViewBag.projectPaymentMethodID = new SelectList(db.PaymentMethodModels, "ID", "projectPaymentMethod");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,projectName,projectCategoryID,projectDescription,projectRequestedDueDate,projectOfferedPaymentAmount,projectPaymentMethodID")] ProjectModels projectModels)
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
            ViewBag.projectPaymentMethodID = new SelectList(db.PaymentMethodModels, "ID", "projectPaymentMethod", projectModels.projectPaymentMethodID);
            return View(projectModels);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,projectName,projectClientID,projectCategoryID,projectDescription,projectRequestedDueDate,projectOfferedPaymentAmount,projectPaymentMethodID,projectStatusID")] ProjectModels projectModels)
        {
            if (ModelState.IsValid)
            {
                if (projectModels.projectStatusID == 2) //calendar change to declined
                {
                    SendEmail(projectModels, "declined");
                }
                if (projectModels.projectStatusID == 5) //calendar change to complete awaiting payment
                {
                    SendEmail(projectModels, "complete");
                }
                if (projectModels.projectStatusID == 6) //calendar change to complete-paid
                {
                    SendEmail(projectModels, "paid");
                }
                db.Entry(projectModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProjectCalendar");
            }
            ViewBag.projectCategoryID = new SelectList(db.CategoryModels, "ID", "categoryName", projectModels.projectCategoryID);
            ViewBag.projectStatusID = new SelectList(db.ProjectStatusModels, "ID", "projectStatusName", projectModels.projectStatusID);
            ViewBag.projectPaymentMethodID = new SelectList(db.PaymentMethodModels, "ID", "projectPaymentMethod", projectModels.projectPaymentMethodID);
            return View(projectModels);
        }

        public ActionResult Change(int? id)
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
            ViewBag.projectPaymentMethodID = new SelectList(db.PaymentMethodModels, "ID", "projectPaymentMethod", projectModels.projectPaymentMethodID);
            return View(projectModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Change([Bind(Include = "ID,projectName,projectClientID,projectCategoryID,projectDescription,projectRequestedDueDate,projectOfferedPaymentAmount,projectPaymentMethodID,projectStatusID")] ProjectModels projectModels)
        {
            if (ModelState.IsValid)
            {
                projectModels.projectStatusID = 1;
                db.Entry(projectModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.projectCategoryID = new SelectList(db.CategoryModels, "ID", "categoryName", projectModels.projectCategoryID);
            ViewBag.projectStatusID = new SelectList(db.ProjectStatusModels, "ID", "projectStatusName", projectModels.projectStatusID);
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

        private void SendEmail(ProjectModels currentModel, string status)
        {
            var fromAddress = new MailAddress("donotreply@livelylit.com", "Lively Literature");
            var toAddress = new MailAddress("jasonhoneyager@gmail.com", "To Name");
            const string fromPassword = "admin1";
            string subject = "";
            if (status == "complete")
            {
                subject = "Invoice - Project: " + currentModel.projectName + " Completed.";
            }
            if (status == "declined")
            {
                subject = "Details - Project: " + currentModel.projectName + " Declined";
            }
            if (status == "paid")
            {
                subject = "Payment Received - Project: " + currentModel.projectName;
            }

            string body = "";
            if (status == "complete")
            {
                body = ConstructInvoice(currentModel);
            }
               
            if (status == "declined")
            {
                body = ConstructDecline(currentModel);
            }
            if (status == "paid")
            {
                body = ConstructThanks(currentModel);
            }

            var smtp = new SmtpClient
            {
                Host = "mail.livelylit.com",
                Port = 587,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        private string ConstructInvoice(ProjectModels currentModel)
        {
            var user = new ApplicationDbContext().Users.Find(User.Identity.GetUserId());
            var client = new ApplicationDbContext().Users.Where(x => x.UserName == currentModel.projectClientID).SingleOrDefault();
            string body = client.ContactName+"\n"+ client.CompanyName + "\n" + client.Street1 + "\n" + client.Street2 + "\n"
                + client.City + ", " + client.State + "\n" + client.Zip + "\n" + client.Phone + "\n\n" + currentModel.projectClientID
                +"\n\r\n\rThank you for choosing Lively Literature Editing and Writing Services.  Your requested project, "
                +currentModel.projectName+", has been completed.  Please remit the agreed upon fee of $"
                +currentModel.projectOfferedPaymentAmount+" within 30 days of receiving this invoice to avoid any additional late payment fees"
                + " I appreciate your business and hope to serve you again in the future!\n\r\n\r" + user.ContactName + "\n\rLively Literature Editing and Writing Services";
            return body;
        }

        private string ConstructDecline(ProjectModels currentModel)
        {
            var user = new ApplicationDbContext().Users.Find(User.Identity.GetUserId());
            var client = new ApplicationDbContext().Users.Where(x => x.UserName == currentModel.projectClientID).SingleOrDefault();
            string body = client.ContactName + "\n" + client.CompanyName + "\n" + client.Street1 + "\n" + client.Street2 + "\n"
                + client.City + ", " + client.State + "\n" + client.Zip + "\n" + client.Phone + "\n\n" + currentModel.projectClientID 
                + "\n\r\n\rThank you for choosing Lively Literature Editing and Writing Services.  I regret to inform you that your requested project, "
                + currentModel.projectName + ", has been has been declined.  Please refer to the following as to the reasoning behind the declination.\n\r\n\r"
                + currentModel.projectOfferedPaymentAmount + "\n\r\n\rIf you would like to renegotiate the terms of the project based on these issues, please feel free to edit the project by using the CHANGE option in the project manager.  The project will remain in the site for 14 days before being removed."
                + " I appreciate your business and hope to serve you again in the future!\n\r\n\r" + user.ContactName + "\n\rLively Literature Editing and Writing Services";
            return body;
        }

        private string ConstructThanks(ProjectModels currentModel)
        {
            var user = new ApplicationDbContext().Users.Find(User.Identity.GetUserId());
            var client = new ApplicationDbContext().Users.Where(x => x.UserName == currentModel.projectClientID).SingleOrDefault();
            string body = client.ContactName + "\n" + client.CompanyName + "\n" + client.Street1 + "\n" + client.Street2 + "\n" 
                + client.City + ", " + client.State + "\n" + client.Zip + "\n" + client.Phone + "\n\n" + currentModel.projectClientID 
                + "\n\r\n\rThank you for choosing Lively Literature Editing and Writing Services.  I have received payment for the Project: "
                + currentModel.projectName + " in the amount of $" + currentModel.projectOfferedPaymentAmount + ".  Thank you for your timely payment."
                + " I appreciate your business and hope to serve you again in the future!\n\r\n\r" + user.ContactName + "\n\rLively Literature Editing and Writing Services";
            return body;
        }
    }
}
