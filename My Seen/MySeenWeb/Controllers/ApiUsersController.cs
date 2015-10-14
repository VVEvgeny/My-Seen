using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySeenWeb;
using MySeenWeb.Models;
using MySeenLib;

namespace MySeenWeb.Controllers
{
    public class ApiUsersController : ApiController
    {
        public IHttpActionResult Get(string user_key, int mode)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            if ((API_Data.ModesApiUsers)mode == API_Data.ModesApiUsers.isUserExists)
            {
                string user_id = string.Empty;
                if(ac.Users.Where(u=>u.UniqueKey == user_key).Count()==0)
                {
                    return Ok(new API_Data.RequestResponseAnswer { Value = API_Data.RequestResponseAnswer.Values.UserNotExist });
                }
                else
                {
                    return Ok(new API_Data.RequestResponseAnswer { Value = API_Data.RequestResponseAnswer.Values.Ok });
                }
            }
            return Ok(new API_Data.RequestResponseAnswer { Value = API_Data.RequestResponseAnswer.Values.BadRequestMode });
        }
    }
}
