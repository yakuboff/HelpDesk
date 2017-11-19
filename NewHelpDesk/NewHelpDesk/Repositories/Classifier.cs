using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewHelpDesk.Models;
using NewHelpDesk.Interfaces;
using System.Data.Entity;


namespace NewHelpDesk.Repositories
{
    public class Classifier<TEntity> : IClassifier<TEntity> where TEntity : class
    {
        protected ApplicationDbContext _db;
        public Classifier(ApplicationDbContext context)
        {
            _db = context;
        }
        public IEnumerable<TEntity> GetClassifier()
        {
            return _db.Set<TEntity>();
        }
    }
}