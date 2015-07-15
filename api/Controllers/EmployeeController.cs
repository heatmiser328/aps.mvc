using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ica.aps.core.interfaces;
using ica.aps.data.interfaces;

namespace ica.aps.api.Controllers
{
    public class EmployeeController : ApiController
    {
        private IEmployeeRepository _repos;

        public EmployeeController(IEmployeeRepository repos)
        {
            _repos = repos;
        }

        // GET api/employee
        public IEnumerable<IEmployee> Get()
        {
            try
            {
                return _repos.GetEmployees();
            }
            catch (Exception ex)
            {
                // introduce a logging system...
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Error retrieving Employees" + System.Environment.NewLine + ex.Message),
                        ReasonPhrase = ex.Message.Replace(System.Environment.NewLine, string.Empty)
                    }
                );
            }
        }

        // GET api/employee/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/employee
        public void Post([FromBody]string value)
        {
        }

        // PUT api/employee/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/employee/5
        public void Delete(int id)
        {
        }
    }
}
