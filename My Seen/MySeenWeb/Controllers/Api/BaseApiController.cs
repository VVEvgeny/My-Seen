using System.Collections.Generic;
using System.Web.Http;
using static MySeenLib.MySeenWebApi;
using static MySeenLib.MySeenWebApi.SyncJsonAnswer;

namespace MySeenWeb.Controllers.Api
{
    public class BaseApiController : ApiController
    {
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
        [HttpPost]
        public IHttpActionResult Post()
        {
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.BadRequestMode });
        }
        [HttpPost]
        public IHttpActionResult Post([FromUri]string userKey)
        {
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.BadRequestMode });
        }
        [HttpPost]
        public IHttpActionResult Post([FromUri]string userKey, [FromUri]int mode)
        {
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.BadRequestMode });
        }
        [HttpPost]
        public IHttpActionResult Post([FromUri]string userKey, [FromUri]int mode, [FromBody] IEnumerable<SyncJsonData> data)
        {
            return Ok(new SyncJsonAnswer { Value = SyncJsonAnswer.Values.NoLongerSupportedVersion });
        }
    }
}
