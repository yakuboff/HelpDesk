using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using NewHelpDesk.Models;




namespace NewHelpDesk.Controllers
{

    public class AdminController : Controller
    {
        RoleManager<IdentityRole> RM = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
        ApplicationDbContext db = new ApplicationDbContext();  
        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        [Authorize(Roles = "Администратор")]
        public ActionResult Users()
        {
            //return View(db.Users.Include(u => u.Roles));
           return View(UserManager.Users.Include(u => u.Roles));
        }
        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public ActionResult Roles()
        {
            
            return View(RM.Roles);
        }

        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult>  CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RM.CreateAsync(new IdentityRole
                {                   
                    Name = model.Name,                   
                });
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            return View();
        }
        [Authorize(Roles = "Администратор")]
        public ActionResult CreateUser()
        {
            //IEnumerable<SelectListItem> roles = db.Roles.Select(b => new SelectListItem { Value = b.Name, Text = b.Name });
            ViewBag.roles = new SelectList(db.Roles, "Name", "Name");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult> CreateUser(UserRoleRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                IdentityResult result =
                    await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {                    
                    await UserManager.AddToRoleAsync(user.Id, model.RoleName.ToString());
                    return RedirectToAction("Users");
                }
                else
                {
                    ViewBag.roles = new SelectList(db.Roles, "Name", "Name");
                    AddErrors(result);
                }
            }
            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
                return RedirectToAction("Users");
            
        }
        [Authorize(Roles = "Администратор")]
        public ActionResult Categories()
        {                      
            return View(db.Categories);
        }

        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public ActionResult CreateCategory(Category category)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Categories.Add(category);
                db.SaveChanges();
            }
            return RedirectToAction("Categories");
        }

    }
}