using System;
using System.Linq;
using System.Web.Http;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using MySeenWeb.Models;
using MySeenLib;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers
{
    public class ApiUsersController : ApiController
    {
        public IHttpActionResult Get(string userKey, int mode, int apiVersion)
        {
            var logger = new NLogLogger();
            const string methodName = "public IHttpActionResult Get(string userKey, int mode, int apiVersion)";
            try
            {
                if (apiVersion != MySeenWebApi.ApiVersion)
                {
                    return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.NoLongerSupportedVersion });
                }
                if ((MySeenWebApi.SyncModesApiUsers)mode == MySeenWebApi.SyncModesApiUsers.IsUserExists)
                {
                    var ac = new ApplicationDbContext();
                    if (!ac.Users.Any(u => u.UniqueKey == userKey))
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
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.SomeErrorObtained });
        }
        public IHttpActionResult Get(string userKey, int mode)
        {
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.NoLongerSupportedVersion });
        }
        public IHttpActionResult Get(string userKey)
        {
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
        }
        public IHttpActionResult Get()
        {
            return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode });
        }
    }
}
