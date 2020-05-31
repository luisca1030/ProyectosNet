using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoListWebApi.Models
{
    public class TasksViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int idStatus { get; set; }
        public int idUsers { get; set; }
        public  StatusViewModel Status { get; set; }
        public  UsersViewModel Users { get; set; }

    }
}