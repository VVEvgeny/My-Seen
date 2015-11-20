using System.Linq;
using System.Web.Http;
using MySeenWeb.Models;
using MySeenLib;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers
{
    public class ApiUsersController : ApiController
    {
        public IHttpActionResult Get(string userKey, int mode, int apiVersion)
        {
            LogSave.Save(userKey, string.Empty, string.Empty, "ApiUsers/Get", mode.ToString());
            if (apiVersion != MySeenWebApi.ApiVersion)
            {
                return Ok(new MySeenWebApi.SyncJsonAnswer { Value = MySeenWebApi.SyncJsonAnswer.Values.NoLongerSupportedVersion });
            }
            if ((MySeenWebApi.SyncModesApiUsers)mode == MySeenWebApi.SyncModesApiUsers.IsUserExists)
            {
                ApplicationDbContext ac = new ApplicationDbContext();
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
