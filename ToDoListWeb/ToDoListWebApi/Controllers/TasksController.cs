using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using ToDoListWebApi.Models;

namespace ToDoListWebApi.Controllers
{
    public class TasksController : Controller
    {
        // GET: Task
        public ActionResult index(string cadena) {

            IEnumerable<TasksViewModel> tasks = null;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:53060/Api/");

                var responseTask = client.GetAsync("tasks?name=" + cadena);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<TasksViewModel>>();
                    readTask.Wait();
                    tasks = readTask.Result;
                }
                else
                {
                    tasks = Enumerable.Empty<TasksViewModel>();
                    ModelState.AddModelError(string.Empty, "Server error");
                }
            }
            return View(tasks);

        }

        public ActionResult Create()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53060/Api/");
                var responseTask = client.GetAsync("status");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<StatusViewModel>>();
                    readTask.Wait();

                    List<SelectListItem> selectItem = new List<SelectListItem>();

                    foreach (var data in readTask.Result)
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = data.Name;
                        item.Value = Convert.ToString(data.Id);
                        item.Selected = false;
                        selectItem.Add(item);
                    }

                    ViewBag.idStatus = selectItem;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error");
                }
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53060/Api/");
                var responseTask = client.GetAsync("users");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IEnumerable<UsersViewModel>>();
                    readTask.Wait();

                    List<SelectListItem> selectItem = new List<SelectListItem>();

                    foreach (var data in readTask.Result)
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = data.Name;
                        item.Value = Convert.ToString(data.Id);
                        item.Selected = false;
                        selectItem.Add(item);
                    }
                    ViewBag.idUsers = selectItem;

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error");
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Create(TasksViewModel tasks)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53060/Api/Tasks");

                var postTask = client.PostAsJsonAsync<TasksViewModel>("Tasks", tasks);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Server error create");

                return View(tasks);
            }
        }

        public ActionResult Edit(int id)
        {
            TasksViewModel task = new TasksViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53060/Api/");

                var responseTask = client.GetAsync("tasks?id=" + id.ToString());

                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<TasksViewModel>();
                    readTask.Wait();

                    task = readTask.Result;
                }
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53060/Api/");
                var responseTask = client.GetAsync("status");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<StatusViewModel>>();
                    readTask.Wait();

                    List<SelectListItem> selectItem = new List<SelectListItem>();

                    foreach (var data in readTask.Result)
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = data.Name;
                        item.Value = Convert.ToString(data.Id);
                        item.Selected =task.Status.Id == data.Id;
                        selectItem.Add(item);
                    }

                    ViewBag.idStatus = selectItem;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error");
                }
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53060/Api/");
                var responseTask = client.GetAsync("users");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IEnumerable<UsersViewModel>>();
                    readTask.Wait();

                    List<SelectListItem> selectItem = new List<SelectListItem>();

                    foreach (var data in readTask.Result)
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = data.Name;
                        item.Value = Convert.ToString(data.Id);
                        item.Selected = task.Users.Id == data.Id;
                        selectItem.Add(item);
                    }
                    ViewBag.idUsers = selectItem;

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error");
                }
            }

            return View(task);
        }

        [HttpPost]
        public ActionResult Edit(TasksViewModel task)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53060/Api/Tasks");

                var postTask = client.PostAsJsonAsync<TasksViewModel>("Tasks", task);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Server error create");
            }

            return View(task);
        }

    }
}