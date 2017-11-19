using System;using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewHelpDesk.Models;
using NewHelpDesk.Interfaces;
using System.Data.Entity;

namespace NewHelpDesk.Repositories
{
    public class UnitOfWork
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RequestRepository requestRepository;
        private UserRepository userRepository;
        private Classifier<Priority> priorityClassifier;
        private Classifier<Status> statusClassifier;
        private Classifier<Category> categoryClassifier;
        public RequestRepository Requests
        {
            get
            {
                if (requestRepository == null)
                    requestRepository = new RequestRepository(db);
                return requestRepository;
            }
        }

        public UserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public Classifier<Priority> Priorities
        {
            get
            {
                if (priorityClassifier == null)
                    priorityClassifier = new Classifier<Priority>(db);
                return priorityClassifier;
            }
        }
        public Classifier<Status> Statuses
        {
            get
            {
                if (statusClassifier == null)
                    statusClassifier = new Classifier<Status>(db);
                return statusClassifier;
            }
        }
        public Classifier<Category> Categories
        {
            get
            {
                if (categoryClassifier == null)
                    categoryClassifier = new Classifier<Category>(db);
                return categoryClassifier;
            }
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}