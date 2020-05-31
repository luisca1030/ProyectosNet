using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using ToDoListWebApi.Models;

namespace ToDoListWebApi.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            IEnumerable<UsersViewModel> users = null;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:53060/Api/");
                var responseTask = client.GetAsync("users");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<UsersViewModel>>();
                    readTask.Wait();
                    users = readTask.Result;
                }
                else
                {

                    users = Enumerable.Empty<UsersViewModel>();
                    ModelState.AddModelError(string.Empty, "Server error");
                }
            }
            return View(users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UsersViewModel user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53060/Api/Users");

                var postTask = client.PostAsJsonAsync<UsersViewModel>("Users", user);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Server error create");

                return View(user);
            }
        }

       
        public ActionResult Edit(int id) {

            UsersViewModel user = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53060/Api/");

                var responseTask = client.GetAsync("users?id=" + id.ToString());

                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<UsersViewModel>();
                    readTask.Wait();

                    user = readTask.Result;
                }
            }

            return View(user);
        }


        [HttpPost]
        public ActionResult Edit(UsersViewModel user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53060/Api/Users");

                var postTask = client.PostAsJsonAsync<UsersViewModel>("Users", user);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

            }
            return View(user);
        }
    }
}