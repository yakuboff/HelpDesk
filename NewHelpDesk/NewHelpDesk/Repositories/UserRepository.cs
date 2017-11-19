using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewHelpDesk.Models;
using NewHelpDesk.Interfaces;
using System.Data.Entity;

namespace NewHelpDesk.Repositories
{
    public class UserRepository:IRepository<ApplicationUser>
    {
        private ApplicationDbContext db;
        public UserRepository(ApplicationDbContext context)
        {
            this.db = context;
        }
        public void Create(ApplicationUser user)
        {
            db.Users.Add(user);
        }
        public void Delete(int id)
        {
            ApplicationUser u = db.Users.Find(id);
            if (u != null)
                db.Users.Remove(u);
        }
        public void Update(ApplicationUser user)
        {
            db.Entry(user).State = EntityState.Modified;
        }
        public IEnumerable<ApplicationUser> GetAll()
        {        
            return db.Users.AsEnumerable();
        }
        public IEnumerable<ApplicationUser> Find(Func<ApplicationUser, Boolean> predicate)
        {
            // Временно
            return null;
        }
        public ApplicationUser Get(int id)
        {
            // Временно
            return null;
        }
        public ApplicationUser GetUser(Func<ApplicationUser, Boolean> predicate)
        {
            return (db.Users.Where(predicate).FirstOrDefault());
        }
    }
}