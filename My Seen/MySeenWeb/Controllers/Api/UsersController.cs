using System;
using System.Linq;
using System.Web.Http;
using MySeenLib;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Models.OtherViewModels;
using static MySeenLib.MySeenWebApi;
using static MySeenLib.MySeenWebApi.SyncJsonAnswer;

namespace MySeenWeb.Controllers.Api
{
    public class UsersController : BaseApiController
    {
        [HttpPost]
        [ActionName("CheckKey")]
        public IHttpActionResult CheckKey([FromUri]int apiVersion, [FromUri]string userKey)
        {
            var logger = new NLogLogger();
            const string methodName = "public IHttpActionResult CheckKey([FromUri]int apiVersion, [FromUri]string userKey)";
            try
            {
                if (!CheckApiVersion(apiVersion))
                {
                    return Ok(new SyncJsonAnswer { Value = Values.NoLongerSupportedVersion, Data = Values.NoLongerSupportedVersion.SplitByWords()});
                }
                var ac = new ApplicationDbContext();
                var user = ac.Users.FirstOrDefault(u => u.UniqueKey == userKey);

                return
                    Ok(user != null
                        ? new SyncJsonAnswer { Value = Values.Ok, Data = user.Email }
                        : new SyncJsonAnswer { Value = Values.UserNotExist, Data = Values.UserNotExist.SplitByWords() });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Ok(new SyncJsonAnswer { Value = Values.SomeErrorObtained, Data = Values.SomeErrorObtained.SplitByWords() });
        }
    }
}
