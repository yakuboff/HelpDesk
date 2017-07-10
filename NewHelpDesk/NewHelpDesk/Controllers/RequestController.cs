using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using NewHelpDesk.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace NewHelpDesk.Controllers
{
    public class RequestController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            ApplicationUser user = db.Users.Where(m => m.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            ViewBag.categories = new SelectList(db.Categories, "Id", "Name");
            ViewBag.priorities = new SelectList(db.Prioritys,"Id","Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Request request)
        {
            ApplicationUser user =  db.Users.Where(m => m.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            request.ApplicationUserId = user.Id;
            request.StatusId = 1;
            db.Requests.Add(request);
            db.SaveChanges();
            return RedirectToAction("MyList");
        }
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var request = db.Requests.Where(r => r.Id == id)
                                     .Include(r => r.Category)
                                     .FirstOrDefault();
            return PartialView("Detail",request);
        }
        public ActionResult ExecutorDetail(string id)
        {
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == id); 
            return PartialView(user);
        }
        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public EmptyResult DeleteRequest(int requestId)
        {
            var request = db.Requests.FirstOrDefault(r => r.Id == requestId);
            db.Requests.Remove(request);
            db.SaveChanges();
            return new EmptyResult();
        }
        [Authorize]
        public ActionResult MyList()
        {
            ApplicationUser user = db.Users.Where(m => m.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            var requests = db.Requests.Where(r => r.ApplicationUserId == user.Id)
                                      .Include(r => r.Category)
                                      .Include(r => r.ApplicationUser)
                                      .Include(r => r.Status)
                                      .Include(r => r.Priority);
            return View(requests);
        }
        public ActionResult RequestList()
        {
            var requests = db.Requests.Include(r => r.Category)
                                         .Include(r => r.ApplicationUser)
                                         .Include(r => r.Executor)
                                         .Include(r => r.Status)
                                         .Include(r => r.Priority);
            return View(requests);
        }
        [HttpGet]
         [Authorize(Roles = "Модератор")]
        public ActionResult Distribute()
        {
            var requests = db.Requests.Include(r => r.Category)
                                         .Include(r => r.ApplicationUser)
                                         .Include(r => r.Executor)
                                         .Include(r => r.Status)
                                         .Include(r => r.Priority);
            List<ApplicationUser> executors = db.Users.ToList();//.Include(e => e.Roles)
                               //.Where(e => e.Roles.FirstOrDefault().RoleId == "Исполнитель").ToList<ApplicationUser>();
            ViewBag.Executors = new SelectList(executors, "Id", "Email");
            return View(requests);
        }
        [HttpPost]
        [Authorize(Roles = "Модератор")]
        public ActionResult Distribute(int? requestId,string executorId)
        {
            if (requestId == null && executorId == null)
            {
                return RedirectToAction("Distribute");
            }
            Request req = db.Requests.Find(requestId);
            ApplicationUser ex = db.Users.Find(executorId);
            if (req == null && ex == null)
            {
                return RedirectToAction("Distribute");
            }
            req.ExecutorId = executorId;

            req.StatusId = 2; //Распределение
            //Lifecycle lifecycle = db.Lifecycles.Find(req.LifecycleId);
            //lifecycle.Distributed = DateTime.Now;
            //db.Entry(lifecycle).State = EntityState.Modified;

            db.Entry(req).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Distribute");
        }


        [HttpGet]
        [Authorize(Roles = "Исполнитель")]
        public ActionResult ChangeStatus()
        {
            // получаем текущего пользователя
            ApplicationUser user = db.Users.Where(m => m.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                var requests = db.Requests.Include(r => r.ApplicationUser)
                                    //.Include(r => r.Lifecycle)
                                    .Include(r => r.Executor)
                                    .Include(r => r.Status)
                                    .Include(r => r.Priority)
                                    .Where(r => r.ExecutorId == user.Id)
                                    .Where(r => r.StatusId != 5);
                List<Status> statuses = db.Statuses.ToList();
                ViewBag.Statuses = new SelectList(statuses, "Id", "Name");
                return View(requests);
            }
            return RedirectToAction("LogOff", "Account");
        }

        [HttpPost]
        [Authorize(Roles = "Исполнитель")]
        public ActionResult ChangeStatus(int requestId, int statusId)
        {
            ApplicationUser user = db.Users.Where(m => m.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            Request req = db.Requests.Find(requestId);
            if (req != null)
            {
                req.StatusId = statusId;
                //Lifecycle lifecycle = db.Lifecycles.Find(req.LifecycleId);
                //if (status == (int)RequestStatus.Proccesing)
                //{
                //    lifecycle.Proccesing = DateTime.Now;
                //}
                //else if (status == (int)RequestStatus.Checking)
                //{
                //    lifecycle.Checking = DateTime.Now;
                //}
                //else if (status == (int)RequestStatus.Closed)
                //{
                //    lifecycle.Closed = DateTime.Now;
                //}
                //db.Entry(lifecycle).State = EntityState.Modified;
                db.Entry(req).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("ChangeStatus");
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
    }
}