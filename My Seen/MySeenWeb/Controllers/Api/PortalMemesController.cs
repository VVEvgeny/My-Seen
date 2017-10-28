using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using static MySeenLib.MySeenWebApi;
using static MySeenLib.MySeenWebApi.SyncJsonAnswer;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.TablesLogic.Portal;

namespace MySeenWeb.Controllers.Api
{
    public class PortalMemesController : BaseApiController
    {
        private readonly ICacheService _cache;

        public PortalMemesController(ICacheService cache)
        {
            _cache = cache;
        }

        //НАДО
        //Научиться читать FromBody
        //в контекстное меню хром адобавить своё действие


        //http://localhost:44301/api/PortalMemes/AddMem/?apiVersion=1&userKey=e290b118702482ec7fd7c1343e02553f&memUri=https://pp.userapi.com/c840634/v840634447/17452/ijxifvXj2VA.jpg
        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("AddMem")]
        //public IHttpActionResult AddMem([FromUri]int apiVersion, [FromUri]string userKey, [FromBody]string json)
        public IHttpActionResult AddMem([FromUri] int apiVersion, [FromUri] string userKey, HttpRequestMessage json)
        {
            try
            {
                if (!CheckApiVersion(apiVersion))
                {
                    return Ok(new SyncJsonAnswer
                    {
                        Value = Values.NoLongerSupportedVersion,
                        Data = Values.NoLongerSupportedVersion.SplitByWords()
                    });
                }
                var ac = new ApplicationDbContext();

                var user = ac.Users.FirstOrDefault(u => u.UniqueKey == userKey);
                if (user != null)
                {
                    var memesLogic = new MemesLogic(_cache);
                    return !memesLogic.Add(" ", json.Content.ReadAsStringAsync().Result, user.Id)
                        ? Ok(new SyncJsonAnswer {Value = Values.SomeErrorObtained, Data = memesLogic.GetError()})
                        : Ok(new SyncJsonAnswer {Value = Values.Ok, Data = json.Content.ReadAsStringAsync().Result });
                }
                return Ok(new SyncJsonAnswer {Value = Values.UserNotExist, Data = Values.UserNotExist.SplitByWords()});
            }
            catch (Exception ex)
            {
                
            }
            return Ok(new SyncJsonAnswer
            {
                Value = Values.SomeErrorObtained,
                //Data = "json="+ json.Content.ReadAsStringAsync().Result
                Data = Values.SomeErrorObtained.SplitByWords()
            });
        }


        /*
        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("AddMem")]
        public IHttpActionResult AddMem(int apiVersion, string userKey, string memUri)
        {
            var logger = new NLogLogger();
            const string methodName =
                "public IHttpActionResult CheckKey([FromUri]int apiVersion, [FromUri]string userKey, [FromBody]string memUri)";
            try
            {
                if (!CheckApiVersion(apiVersion))
                {
                    return Ok(new SyncJsonAnswer
                    {
                        Value = Values.NoLongerSupportedVersion,
                        Data = Values.NoLongerSupportedVersion.SplitByWords()
                    });
                }
                var ac = new ApplicationDbContext();

                var user = ac.Users.FirstOrDefault(u => u.UniqueKey == userKey);
                if (user != null)
                {
                    var memesLogic = new MemesLogic(_cache);
                    return !memesLogic.Add(" ", memUri, user.Id)
                        ? Ok(new SyncJsonAnswer {Value = Values.SomeErrorObtained, Data = memesLogic.GetError()})
                        : Ok(new SyncJsonAnswer {Value = Values.Ok, Data = user.Email});
                }
                return Ok(new SyncJsonAnswer {Value = Values.UserNotExist, Data = Values.UserNotExist.SplitByWords()});
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Ok(new SyncJsonAnswer
            {
                Value = Values.SomeErrorObtained,
                Data = Values.SomeErrorObtained.SplitByWords()
            });
        }
        */
    }
}
