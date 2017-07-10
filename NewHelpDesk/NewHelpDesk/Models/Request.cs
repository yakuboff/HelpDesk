using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewHelpDesk.Models;
using System.ComponentModel.DataAnnotations;

namespace NewHelpDesk.Models
{
    public class Request
    {
        public int Id { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }
        [Display(Name = "Статус")]
        public int? StatusId { get; set; }        
        public Status Status {get;set;}
        [Display(Name = "Приоритет")]
        public int? PriorityId { get; set; }
        public Priority Priority {get;set;}
        [Display(Name = "Категория")]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        [Display(Name = "Пользователь")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "Исполнитель")]
        public string ExecutorId { get; set; }
        public ApplicationUser Executor { get; set; }

    }

    public class Category
    {
        [Display(Name = "Ид")]        
        public int Id {get;set;}
        [Display(Name = "Наименование")]
        public string Name {get;set;}
    }
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Priority
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}