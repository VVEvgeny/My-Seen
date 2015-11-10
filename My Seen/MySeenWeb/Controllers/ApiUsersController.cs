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
        public IHttpActionResult Get(string user_key, int mode, int apiVersion)
        {
            LogSave.Save(user_key, string.Empty, string.Empty, "ApiUsers/Get", mode.ToString());
            if (apiVersion != MySeenWebApi.ApiVersion)
            {
                return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.NoLongerSupportedVersion });
            }
            if ((MySeenWebApi.SyncModesApiUsers)mode == MySeenWebApi.SyncModesApiUsers.isUserExists)
            {
                string user_id = string.Empty;
                ApplicationDbContext ac = new ApplicationDbContext();
                if (ac.Users.Where(u => u.UniqueKey == user_key).Count() == 0)
                {
                    return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.UserNotExist });
                }
                else
                {
                    return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.Ok });
                }
            }
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
        }
        public IHttpActionResult Get(string user_key, int mode)
        {
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.NoLongerSupportedVersion });
        }
        public IHttpActionResult Get(string user_key)
        {
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
        }
        public IHttpActionResult Get()
        {
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
        }
    }
}
