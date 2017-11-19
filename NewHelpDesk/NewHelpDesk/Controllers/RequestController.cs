using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using NewHelpDesk.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using NewHelpDesk.Interfaces;
using NewHelpDesk.Repositories;

namespace NewHelpDesk.Controllers
{
    public class RequestController : Controller
    {
        UnitOfWork UoW;
        public RequestController()
        {
            UoW = new UnitOfWork();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            ApplicationUser user = UoW.Users.GetUser(m => m.Email == HttpContext.User.Identity.Name);
            ViewBag.categories = new SelectList(UoW.Categories.GetClassifier(), "Id", "Name");
            ViewBag.priorities = new SelectList(UoW.Priorities.GetClassifier(),"Id","Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Request request)
        {
            ApplicationUser user = UoW.Users.GetUser(m => m.Email == HttpContext.User.Identity.Name);
            request.ApplicationUserId = user.Id;
            request.StatusId = 1;
            UoW.Requests.Create(request);
            UoW.Save();
            return RedirectToAction("MyList");
        }
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var request = UoW.Requests.Get(id);
            return PartialView("Detail",request);
        }
        public ActionResult ExecutorDetail(string id)
        {
            ApplicationUser user = UoW.Users.GetUser(u => u.Id == id); 
            return PartialView(user);
        }
        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public EmptyResult DeleteRequest(int requestId)
        {          
            UoW.Requests.Delete(requestId);
            UoW.Save();
            return new EmptyResult();
        }
        [Authorize]
        public ActionResult MyList()
        {
            ApplicationUser user = UoW.Users.GetUser(m => m.Email == HttpContext.User.Identity.Name);
            var requests = UoW.Requests.Find(r => r.ApplicationUserId == user.Id);                                      
            return View(requests);
        }
        public ActionResult RequestList()
        {
            return View(UoW.Requests.GetAll());
        }
        [HttpGet]
        [Authorize(Roles = "Модератор")]
        public ActionResult Distribute()
        {
            var requests = UoW.Requests.GetAll();
            ViewBag.Executors = new SelectList(UoW.Users.GetAll(), "Id", "Email");
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
            Request req = UoW.Requests.Get((int)requestId);
            ApplicationUser ex = UoW.Users.GetUser(r => r.Id == executorId);
            if (req == null && ex == null)
            {
                return RedirectToAction("Distribute");
            }
            req.ExecutorId = executorId;

            req.StatusId = 2; //Распределение
            UoW.Requests.Update(req);
            UoW.Save();
            return RedirectToAction("Distribute");
        }


        [HttpGet]
        [Authorize(Roles = "Исполнитель")]
        public ActionResult ChangeStatus()
        {
            // получаем текущего пользователя
            ApplicationUser user = UoW.Users.GetUser(m => m.Email == HttpContext.User.Identity.Name);
            if (user != null)
            {
                var requests = UoW.Requests.Find(r => (r.ExecutorId == user.Id && r.ExecutorId == user.Id));
                ViewBag.Statuses = new SelectList(UoW.Users.GetAll(), "Id", "Name");
                return View(requests);
            }
            return RedirectToAction("LogOff", "Account");
        }

        [HttpPost]
        [Authorize(Roles = "Исполнитель")]
        public ActionResult ChangeStatus(int requestId, int statusId)
        {
            ApplicationUser user = UoW.Users.GetUser(m => m.Email == HttpContext.User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            Request req = UoW.Requests.Get(requestId);
            if (req != null)
            {
                req.StatusId = statusId;
                UoW.Requests.Update(req);
                UoW.Save();
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