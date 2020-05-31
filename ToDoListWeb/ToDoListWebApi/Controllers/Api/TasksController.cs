using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ToDoListWebApi.Models;

namespace ToDoListWebApi.Controllers.Api
{
    public class TasksController : ApiController
    {
        /// <summary>
        /// Get Tasks by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IHttpActionResult GetAllTasks(string name)
        {
            IList<TasksViewModel> tasks = null;
            using (var model = new DataModelToDo())
            {
                if (!string.IsNullOrEmpty(name))
                {
                    tasks = model.Tasks.Include("status").Include("users")
                        .Where(s => s.Name.Contains(name))
                        .Select(s => new TasksViewModel()
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Description = s.Description,
                            Status = s.Status == null ? null : new StatusViewModel()
                            {
                                Id = s.Status.Id,
                                Name = s.Status.Name
                            },
                            Users = s.Users == null ? null : new UsersViewModel()
                            {
                                Id = s.Users.Id,
                                Name = s.Users.Name,
                                PhoneNumber = s.Users.PhoneNumber,
                                Email = s.Users.Email,
                            }
                        }).ToList<TasksViewModel>();
                }
                else {

                    tasks = model.Tasks.Include("status").Include("users")
                    .Select(s => new TasksViewModel()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        Status = s.Status == null ? null : new StatusViewModel()
                        {
                            Id = s.Status.Id,
                            Name = s.Status.Name
                        },
                        Users = s.Users == null ? null : new UsersViewModel()
                        {
                            Id = s.Users.Id,
                            Name = s.Users.Name,
                            PhoneNumber = s.Users.PhoneNumber,
                            Email = s.Users.Email,
                        }
                    }).ToList<TasksViewModel>();
                }
            }
            if (tasks.Count == 0)
            {
                return NotFound();
            }
            return Ok(tasks);
        }


        /// <summary>
        /// Get Tasks by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult GetAllTasks(int id)
        {
            TasksViewModel tasks = null;
            using (var model = new DataModelToDo())
            {
                tasks = model.Tasks.Include("status").Include("users")
                    .Where(s => s.Id == id)
                    .Select(s => new TasksViewModel()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        idStatus = s.idStatus,
                        Status = s.Status == null ? null : new StatusViewModel()
                        {
                            Id = s.Status.Id,
                            Name = s.Status.Name
                        },
                        idUsers =s.idUsers,
                        Users = s.Users == null ? null : new UsersViewModel()
                        {
                            Id = s.Users.Id,
                            Name = s.Users.Name,
                            PhoneNumber = s.Users.PhoneNumber,
                            Email = s.Users.Email,
                        }
                        
                    }).FirstOrDefault<TasksViewModel>();
            }

            if (tasks == null)
            {
                return NotFound();
            }
            return Ok(tasks);
        }

        /// <summary>
        /// Create and Edit Task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public IHttpActionResult PostTasks(TasksViewModel task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            using (var model = new DataModelToDo())
            {
                var existingtask = model.Tasks.Where(s => s.Id == task.Id).FirstOrDefault<Tasks>();

                if (existingtask != null)
                {
                    existingtask.Name = task.Name;
                    existingtask.Description = task.Description;
                    existingtask.idStatus = task.idStatus;
                    existingtask.idUsers = task.idUsers;

                    model.SaveChanges();
                }
                else if(task.Id == 0)
                {
                    model.Tasks.Add(new Tasks()
                    {
                        Name = task.Name,
                        Description = task.Description,
                        idStatus = task.idStatus,
                        idUsers = task.idUsers
                    });
                }
                else
                {
                    return NotFound();
                }
                model.SaveChanges();
            }
            return Ok();
        }
    }
}
