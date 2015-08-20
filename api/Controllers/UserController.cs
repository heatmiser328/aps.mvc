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
    public class UserController : ApiController
    {
        private IUserRepository _repos;

        public UserController(IUserRepository repos)
        {
            _repos = repos;
        }

        // GET api/user/foo
        public IUser Get(string username)
        {
            try
            {
                return _repos.GetUser(username);
            }
            catch (Exception ex)
            {
                // introduce a logging system...
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Error retrieving User" + System.Environment.NewLine + ex.Message),
                        ReasonPhrase = ex.Message.Replace(System.Environment.NewLine, string.Empty)
                    }
                );
            }
        }
    }
}
