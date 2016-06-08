using System;
using System.Linq;
using System.Web.Http;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Models.OtherViewModels;
using static MySeenLib.MySeenWebApi;
using static MySeenLib.MySeenWebApi.SyncJsonAnswer;
using MySeenWeb.Add_Code;

namespace MySeenWeb.Controllers.Api
{
    public class UsersController : BaseApiController
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
                if ((SyncModesApiUsers) mode == SyncModesApiUsers.IsUserExists)
                {
                    var ac = new ApplicationDbContext();
                    return
                        Ok(ac.Users.Select(u => u.Id).ToList().Any(s => Md5Tools.Get(s) == userKey)
                            ? new SyncJsonAnswer {Value = Values.Ok}
                            : new SyncJsonAnswer {Value = Values.UserNotExist});
                }
                return Ok(new SyncJsonAnswer { Value = Values.BadRequestMode });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Ok(new SyncJsonAnswer { Value = Values.SomeErrorObtained });
        }
    }
}
