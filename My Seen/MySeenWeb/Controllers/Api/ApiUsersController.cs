using System;
using System.Linq;
using System.Web.Http;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Models.OtherViewModels;
using static MySeenLib.MySeenWebApi;
using static MySeenLib.MySeenWebApi.SyncJsonAnswer;

namespace MySeenWeb.Controllers.Api
{
    public class ApiUsersController : ApiController
    {
        public IHttpActionResult Get(string userKey, int mode, int apiVersion)
        {
            var logger = new NLogLogger();
            const string methodName = "public IHttpActionResult Get(string userKey, int mode, int apiVersion)";
            try
            {
                if (apiVersion != ApiVersion)
                {
                    return Ok(new SyncJsonAnswer { Value = Values.NoLongerSupportedVersion });
                }
                if ((SyncModesApiUsers)mode == SyncModesApiUsers.IsUserExists)
                {
                    var ac = new ApplicationDbContext();
                    if (!ac.Users.Any(u => u.UniqueKey == userKey))
                    {
                        return Ok(new SyncJsonAnswer { Value = Values.UserNotExist });
                    }
                    else
                    {
                        return Ok(new SyncJsonAnswer { Value = Values.Ok });
                    }
                }
                return Ok(new SyncJsonAnswer { Value = Values.BadRequestMode });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Ok(new SyncJsonAnswer { Value = Values.SomeErrorObtained });
        }
        public IHttpActionResult Get(string userKey, int mode)
        {
            return Ok(new SyncJsonAnswer { Value = Values.NoLongerSupportedVersion });
        }
        public IHttpActionResult Get(string userKey)
        {
            return Ok(new SyncJsonAnswer { Value = Values.BadRequestMode });
        }
        public IHttpActionResult Get()
        {
            return Ok(new SyncJsonAnswer { Value = Values.BadRequestMode });
        }
    }
}
