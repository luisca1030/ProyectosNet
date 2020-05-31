using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoListWebApi.Models
{
    public class UsersViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long? PhoneNumber { get; set; }
        public string Email { get; set; }
        
    }
}