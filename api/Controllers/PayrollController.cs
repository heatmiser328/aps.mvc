using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

using ica.aps.core.interfaces;
using ica.aps.data.models;


namespace ica.aps.api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PayrollController : ApiController
    {
        private IPayrollManager _mgr;

        public PayrollController(IPayrollManager mgr)
        {
            _mgr = mgr;
        }

        // GET api/payroll/start/{startDate}
        [HttpGet]
        public HttpResponseMessage Get(DateTime startDate)
        {
            try
            {
                var payroll = _mgr.GetPayroll(startDate);
                return Request.CreateResponse(HttpStatusCode.OK, payroll, "application/json");
            }
            catch (Exception ex)
            {
                // introduce a logging system...
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Error retrieving Payroll" + System.Environment.NewLine + ex.Message),
                        ReasonPhrase = ex.Message.Replace(System.Environment.NewLine, string.Empty)
                    }
                );
            }
        }

        // POST api/payroll
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Payroll payroll)
        {
            try
            {
                _mgr.SavePayroll(payroll);
                return Request.CreateResponse(HttpStatusCode.OK, payroll, "application/json");
            }
            catch (Exception ex)
            {
                // introduce a logging system...
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Error saving Payroll" + System.Environment.NewLine + ex.Message),
                        ReasonPhrase = ex.Message.Replace(System.Environment.NewLine, string.Empty)
                    }
                );
            }
        }

        /*
        // PUT api/payroll/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/payroll/5
        public void Delete(int id)
        {
        }
        */ 
    }
}
