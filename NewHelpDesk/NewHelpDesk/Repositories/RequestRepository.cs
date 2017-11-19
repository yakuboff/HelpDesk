using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewHelpDesk.Models;
using NewHelpDesk.Interfaces;
using System.Data.Entity;

namespace NewHelpDesk.Repositories
{
    public class RequestRepository:IRepository<Request>
    {
        private ApplicationDbContext db;
        public RequestRepository(ApplicationDbContext context)
        {
            this.db = context;
        }
        public void Create(Request request)
        {
            db.Requests.Add(request);
        }
        public void Delete(int id)
        {
            Request r = db.Requests.Find(id);
            if (r != null) db.Requests.Remove(r);
        }
        public void Update(Request request)
        {
            db.Entry(request).State = EntityState.Modified;
        }
        public IEnumerable<Request> GetAll()
        {
            return (db.Requests.Include(r => r.Category)
                                         .Include(r => r.ApplicationUser)
                                         .Include(r => r.Executor)
                                         .Include(r => r.Status)
                                         .Include(r => r.Priority));
        }
        public IEnumerable<Request> Find(Func<Request, Boolean> predicate)
        {
            return (db.Requests.Include(r => r.ApplicationUser)
                               .Include(r => r.Category)
                               .Include(r => r.Executor)
                               .Include(r => r.Status)
                               .Include(r => r.Priority)
                                .Where(predicate));
        }
        public Request Get(int id)
        {
            return (db.Requests.Include(r => r.Category).FirstOrDefault(r => r.Id == id));
        }
    }
}