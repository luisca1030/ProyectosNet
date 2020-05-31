using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ToDoListWebApi.Models;

namespace ToDoListWebApi.Controllers.Api
{
    /// <summary>
    /// Get Status
    /// </summary>
    public class StatusController : ApiController
    {
        public IHttpActionResult GetAllStatus()
        {
            IList<StatusViewModel> status = null;
            using (var model = new DataModelToDo())
            {
                status = model.Status
                    .Select(s => new StatusViewModel()
                    {
                        Id = s.Id,
                        Name = s.Name

                    }).ToList<StatusViewModel>();
            }

            if (status.Count == 0)
            {
                return NotFound();
            }

            return Ok(status);
        }
    }
}
