using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ToDoListWebApi.Models;

namespace ToDoListWebApi.Controllers.Api
{
    public class UsersController : ApiController
    {
        /// <summary>
        /// Get Users
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetAllUser()
        {
            IList<UsersViewModel> users = null;
            using (var model = new DataModelToDo())
            {
                users = model.Users.Include("Users")
                    .Select(s => new UsersViewModel()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Email = s.Email,
                        PhoneNumber = s.PhoneNumber
                    }).ToList<UsersViewModel>();
            }

            if (users.Count == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }

        /// <summary>
        /// Get User For Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult GetUserById(int id) {

            UsersViewModel user = null;

            using (var model = new DataModelToDo())
            {
                user = model.Users
                    .Where(s => s.Id == id)
                    .Select(s => new UsersViewModel()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Email = s.Email,
                        PhoneNumber = s.PhoneNumber                       
                    }).FirstOrDefault<UsersViewModel>();
            }

            if (user == null) {
                return NotFound();
            }

            return Ok(user);
        }
       
        
        /// <summary>
        /// Create and Edit User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IHttpActionResult PostUser(UsersViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            using (var model = new DataModelToDo())
            {
                var existingUser = model.Users.Where(s => s.Id == user.Id).FirstOrDefault<Users>();

                if (existingUser != null)
                {
                    existingUser.Name = user.Name;
                    existingUser.Email = user.Email;
                    existingUser.PhoneNumber = user.PhoneNumber; 
                }
                else if (user.Id == 0)
                {
                    model.Users.Add(new Users()
                    {
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber
                    });
                }
                else {
                    return NotFound();
                }

                model.SaveChanges();
            }

            return Ok();
        }
    }
}
